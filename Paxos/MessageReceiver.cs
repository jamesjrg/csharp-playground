using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Paxos.Messages;

namespace Paxos
{
    [Route("paxos")]
    [ApiController]
    public class MessageReceiver : ControllerBase
    {
        Queues _queues;

        public MessageReceiver(Queues queues)
        {
            _queues = queues;
        }
        
        [HttpPost("prepare/{target}")]
        public void Post(string target, [FromBody]Prepare message)
        {
            _queues.AcceptorQueues[target].Enqueue(message);
        }
        
        [HttpPost("accepted/{target}")]
        public void Post(string target, [FromBody]Accepted message)
        {
            _queues.LearnerQueues[target].Enqueue(message);
        }
        
        [HttpPost("promised/{target}")]
        public void Post(string target, [FromBody]Promised message)
        {
            _queues.ProposerQueues[target].Enqueue(message);
        }
        
        [HttpPost("proposed/{target}")]
        public void Post(string target, [FromBody]Proposed message)
        {
            _queues.AcceptorQueues[target].Enqueue(message);
        }
    }
}