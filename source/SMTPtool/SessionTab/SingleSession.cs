using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMTPtool;

namespace SMTPtestTool
{
    public class SingleSession
    {
       // private String commandToSend;


        Main _linkToMain;

        public SingleSession(Main _linkToMain)
        {
            this._linkToMain = _linkToMain;
        }


        public void connect() { }

        public void start() { }

        public void run() { }

        private String Read(){
            return "";
        }

        private void Write() { }

        private void clearHistory() { }

        private void addHistoryEntry(String entry) { }

    }
}
