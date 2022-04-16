using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;
using WPFGameTest.Helpers;
using WPFGameTest.Logic;
using WPFGameTest.Models;

namespace WPFGameTest
{
    public struct InputPayload
    {
        public Vector2f inputVector;
        public float deltaTime;
        public int tick;
    }
    public struct StatePayload
    {
        public Vector2 position;
        public int tick;
    }

    public class TestClient
    {
        private static readonly TestClient instance = new TestClient();

        // Shared
        private int currentTick;
        private const int BUFFER_SIZE = 1024;

        private StatePayload[] stateBuffer;
        private InputPayload[] inputBuffer;
        private StatePayload latestServerState;
        private StatePayload lastProcessedState;
        private float horizontalInput;
        private float verticalInput;

        private Player player;

        static TestClient() { }

        private TestClient() { }

        public static TestClient Instance
        {
            get
            {
                return instance;
            }
        }

        public void Init(Player player)
        {
            this.player = player;

            stateBuffer = new StatePayload[BUFFER_SIZE];
            inputBuffer = new InputPayload[BUFFER_SIZE];
        }

        public void Update()
        {
            if (Input.GetKey(Key.A)) horizontalInput = -1;
            else if (Input.GetKey(Key.D)) horizontalInput = 1;
            else horizontalInput = 0;

            if (Input.GetKey(Key.W)) verticalInput = -1;
            else if (Input.GetKey(Key.S)) verticalInput = 1;
            else verticalInput = 0;

            HandleTick();
            currentTick++;
        }

        private void HandleTick()
        {
            if (!latestServerState.Equals(default(StatePayload)) &&
                (lastProcessedState.Equals(default(StatePayload)) ||
                !latestServerState.Equals(lastProcessedState)))
            {
                HandleServerReconciliation();
            }

            // If there is no input just return
            if (horizontalInput == 0 && verticalInput == 0)
                return;

            int bufferIndex = currentTick % BUFFER_SIZE;

            // Add payload to inputBuffer
            InputPayload inputPayload = new InputPayload()
            {
                tick = currentTick,
                inputVector = new Vector2f(horizontalInput, verticalInput),
                deltaTime = Time.DeltaTime
            };

            inputBuffer[bufferIndex] = inputPayload;

            // Add payload to statebuffer
            stateBuffer[bufferIndex] = ProcessMovement(inputPayload);

            // Send input to server
            SendToServer(inputPayload);
        }

        private void HandleServerReconciliation()
        {
            lastProcessedState = latestServerState;

            int serverStateBufferIndex = latestServerState.tick % BUFFER_SIZE;

            float positionError = Vector2.Distance(latestServerState.position, stateBuffer[serverStateBufferIndex].position); // Get difference between the current state and latest server state positions

            if (positionError > 0.00001f)
            {
                Trace.WriteLine("Need to reconcile");
                // Rewind and Replay
                player.Transform.Position = latestServerState.position;

                // Update buffer at index of latest server state
                stateBuffer[serverStateBufferIndex] = latestServerState;

                // Now re-simulate the rest of the ticks up to the current tick on the client
                int tickToProcess = latestServerState.tick + 1;

                while (tickToProcess < currentTick)
                {
                    // Process new movement with reconciled state
                    StatePayload statePayload = ProcessMovement(inputBuffer[tickToProcess]);

                    // Update buffer with recalculated state
                    int bufferIndex = tickToProcess % BUFFER_SIZE;
                    stateBuffer[bufferIndex] = statePayload;

                    tickToProcess++;
                }
            }
        }

        private StatePayload ProcessMovement(InputPayload inputPayload)
        {
            player.MoveY(verticalInput * 250f * inputPayload.deltaTime, null);
            player.MoveX(horizontalInput * 250f * inputPayload.deltaTime, null);

            return new StatePayload()
            {
                tick = inputPayload.tick,
                position = player.Transform.Position
            };
        }

        public void OnServerMovementState(StatePayload serverState)
        {
            latestServerState = serverState;
        }

        private void SendToServer(InputPayload inputPayload)
        {
            TestServer.Instance.OnClientInput(inputPayload);
        }
    }
}
