namespace GZH.Agent.Manager.Controllers
{
    public class MsgEntity
    {
        public int returnCode { get; set; }

        public string message { get; set; }
    }

    public class ResponseMsg
    {
        public static MsgEntity SetEntity(out MsgEntity r,  int returnCode)
        {
            r = new MsgEntity();

            //CUD
            switch (returnCode)
            {
                //Create
                case 1000:
                    r.returnCode = returnCode;
                    r.message = "添加成功";
                    break;
                case 1100:
                    r.returnCode = returnCode;
                    r.message = "添加失败，记录已存在";
                    break;
                case 1101:
                    r.returnCode = returnCode;
                    r.message = "添加失败，记录写入出错";
                    break;
                case 1102:
                    r.returnCode = returnCode;
                    r.message = "添加失败，记录id异常";
                    break;
                case 1103:
                    r.returnCode = returnCode;
                    r.message = "添加失败，记录id";
                    break;

                //Update
                case 2000:
                    r.returnCode = returnCode;
                    r.message = "修改成功";
                    break;
                case 2100:
                    r.returnCode = returnCode;
                    r.message = "修改失败，记录不存在";
                    break;
                case 2101:
                    r.returnCode = returnCode;
                    r.message = "修改失败，记录写入出错";
                    break;

                //Del
                case 3000:
                    r.returnCode = returnCode;
                    r.message = "删除成功";
                    break;
                case 3100:
                    r.returnCode = returnCode;
                    r.message = "删除失败，记录不存在";
                    break;
                case 3101:
                    r.returnCode = returnCode;
                    r.message = "删除失败，记录写入出错";
                    break;

                //Login
                case 4000:
                    r.returnCode = returnCode;
                    r.message = "登录成功";
                    break;
                case 4100:
                    r.returnCode = returnCode;
                    r.message = "登录失败，账号密码有误";
                    break;
                case 4101:
                    r.returnCode = returnCode;
                    r.message = "登录失败，账号失效";
                    break;
                case 4102:
                    r.returnCode = returnCode;
                    r.message = "登录超时";
                    break;
                case 4201:
                    r.returnCode = returnCode;
                    r.message = "注销成功";
                    break;

                //Mail
                case 5000:
                    r.returnCode = returnCode;
                    r.message = "邮件已发送";
                    break;
                case 5101:
                    r.returnCode = returnCode;
                    r.message = "邮件发送失效";
                    break;
            }

            return r;
        }
    }
}