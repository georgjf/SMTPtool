using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMTPtestTool
{
    public abstract class SingleSessionHandler_Abstract
    {

        public abstract void receiveCommand(String receivedCommand);

        public abstract void sendCommand(String commandToSend);

        public abstract void updateHistory(List<string> hisItems);

    }
}
