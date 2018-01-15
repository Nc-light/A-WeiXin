namespace GZH.CL.JsSDK.Entity
{
    public class TicketEntity
    {
        private int _errcode;

        public int errcode
        {
            get { return _errcode; }
            set { _errcode = value; }
        }
        private string _errmsg;

        public string errmsg
        {
            get { return _errmsg; }
            set { _errmsg = value; }
        }
        private string _ticket;

        public string ticket
        {
            get { return _ticket; }
            set { _ticket = value; }
        }
        private int _expires_in;

        public int expires_in
        {
            get { return _expires_in; }
            set { _expires_in = value; }
        }
    }
}
