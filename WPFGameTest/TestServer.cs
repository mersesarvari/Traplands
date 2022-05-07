using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Helpers;
using Game.Models;

namespace Game
{
    public class TestServer
    {
        private static readonly TestServer instance = new TestServer();

        private const float SERVER_TICK_RATE = 30f;
        private float timer;
        private int currentTick;
        private float minTimeBetweenTicks;
        private const int BUFFER_SIZE = 1024;

        private StatePayload[] stateBuffer;
        private Queue<InputPayload> inputQueue;

        private Player player;

        static TestServer() { }

        private TestServer() { }

        public static TestServer Instance
        {
            get
            {
                return instance;
            }
        }

        public void Init(Player player)
        {
            this.player = player;
            minTimeBetweenTicks = 1f / SERVER_TICK_RATE;

            stateBuffer = new StatePayload[BUFFER_SIZE];
            inputQueue = new Queue<InputPayload>();
        }


        public void Update()
        {
            timer += Time.DeltaTime;

            while (timer >= minTimeBetweenTicks)
            {
                timer -= minTimeBetweenTicks;
                HandleTick();
                currentTick++;
            }
        }

        public void HandleTick()
        {
            // Process input queue
            int bufferIndex = -1;

            while (inputQueue.Count > 0)
            {
                InputPayload inputPayload = inputQueue.Dequeue();

                bufferIndex = inputPayload.tick % BUFFER_SIZE;

                StatePayload statePayload = ProcessMovement(inputPayload);
                stateBuffer[bufferIndex] = statePayload;
            }

            if (bufferIndex != -1)
            {
                SendToClient(stateBuffer[bufferIndex]);
            }
        }

        private StatePayload ProcessMovement(InputPayload inputPayload)
        {
            player.MoveY(inputPayload.inputVector.Y * 250f * inputPayload.deltaTime, null);
            player.MoveX(inputPayload.inputVector.X * 250f * inputPayload.deltaTime, null);

            return new StatePayload()
            {
                tick = inputPayload.tick,
                position = player.Transform.Position
            };
        }

        public void OnClientInput(InputPayload inputPayload)
        {
            inputQueue.Enqueue(inputPayload);
        }

        private void SendToClient(StatePayload statePayload)
        {
            TestClient.Instance.OnServerMovementState(statePayload);
        }
    }
}
