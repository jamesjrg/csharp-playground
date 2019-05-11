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
        
        [HttpPost("accepted/{target:str}")]
        public void Post(string target, [FromBody]Accepted message)
        {
        }
        
        [HttpPost("promised/{target:str}")]
        public void Post(string target, [FromBody]Promised message)
        {
        }
        
        [HttpPost("proposed/{target:str}")]
        public void Post(string target, [FromBody]Proposed message)
        {
        }
    }
}