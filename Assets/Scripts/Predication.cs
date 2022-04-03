using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class Predication : MonoBehaviour
    {
        public struct ClientState
        {
            public Vector3 positionVector;
            public Vector3 rotationVector;
        }

        private struct StateMessage
        {
            public float delivery_time;
            public uint tick_number;
            public Vector3 position;
            public Vector3 rotation;
        }
        
        public struct InputMessage
        {
            public float deliveryTime;
            public uint startTickNumber;

            public List<ClientState> inputs;
        }

        private GameObject server_display_player => _player.gameObject;

        public float latency = 0.1f;
        public float packet_loss_chance = 0.05f;

        private Queue<StateMessage> client_state_msgs;

        public uint server_snapshot_rate;
        private uint server_tick_number;
        private uint server_tick_accumulator;
        private Queue<InputMessage> server_input_msgs;

        private Player _player;

        private void Start()
        {
            this.server_tick_number = 0;
            this.server_tick_accumulator = 0;
            this.server_input_msgs = new Queue<InputMessage>();
        }

        public void SetPlayer(Player player) => _player = player;

        private void Update()
        {
            uint server_tick_number = this.server_tick_number;
            uint server_tick_accumulator = this.server_tick_accumulator;

            while (this.server_input_msgs.Count > 0 && Time.time >= this.server_input_msgs.Peek().deliveryTime)
            {
                InputMessage input_msg = this.server_input_msgs.Dequeue();

                // message contains an array of inputs, calculate what tick the final one is
                uint max_tick = input_msg.startTickNumber + (uint)input_msg.inputs.Count - 1;

                // if that tick is greater than or equal to the current tick we're on, then it
                // has inputs which are new
                if (max_tick >= server_tick_number)
                {
                    // there may be some inputs in the array that we've already had,
                    // so figure out where to start
                    uint start_i = server_tick_number > input_msg.startTickNumber ? (server_tick_number - input_msg.startTickNumber) : 0;

                    // run through all relevant inputs, and step player forward
                    for (int i = (int)start_i; i < input_msg.inputs.Count; ++i)
                    {
                        _player.SetInput(input_msg.inputs[i].positionVector, input_msg.inputs[i].rotationVector);

                        ++server_tick_number;
                        ++server_tick_accumulator;
                        if (server_tick_accumulator >= this.server_snapshot_rate)
                        {
                            server_tick_accumulator = 0;

                            if (UnityEngine.Random.value > this.packet_loss_chance)
                            {
                                StateMessage state_msg;
                                state_msg.delivery_time = Time.time + this.latency;
                                state_msg.tick_number = server_tick_number;
                                state_msg.position = _player.transform.position;
                                state_msg.rotation = _player.transform.forward;
                                this.client_state_msgs.Enqueue(state_msg);
                            }
                        }
                    }

                    this.server_display_player.transform.position = _player.transform.position;
                    this.server_display_player.transform.rotation = _player.transform.rotation;
                }
            }

            this.server_tick_number = server_tick_number;
            this.server_tick_accumulator = server_tick_accumulator;
        }
    }


}
