namespace SnsApi.Entity
{
    public class BaseEntity
    {
        private string _access_token;
        public string access_token
        {
            get { return _access_token; }
            set { _access_token = value; }
        }

        private int _expires_in;
        public int expires_in
        {
            get { return _expires_in; }
            set { _expires_in = value; }
        }

        private string _refresh_token;
        public string refresh_token
        {
            get { return _refresh_token; }
            set { _refresh_token = value; }
        }

        private string _openid;
        public string openid
        {
            get { return _openid; }
            set { _openid = value; }
        }

        private string _scope;
        public string scope
        {
            get { return _scope; }
            set { _scope = value; }
        }

        //private string _unionid;
        //public string unionid
        //{
        //    get { return _unionid; }
        //    set { _unionid = value; }
        //}

        //private string _state;
        //public string state
        //{
        //    get { return _state; }
        //    set { _state = value; }
        //}
    }
}
