using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    
    
        //建立“管理员”实体类Admin
        public class Administrator
    {
            [Key]
            public int ID { get; set; }

            public string Name { get; set; }//姓名
            public string Password { get; set; }//登录密码
            public string Phone { get; set; }
            public string Right1 { get; set; }//权限
        }

        //建立“固定用户”实体类SecretUser
        public class SecretUser
        {
            [Key]
            public int ID { get; set; }

            public string Name { get; set; }//姓名
            public string Gender { get; set; }//性别
            public string CardID { get; set; }//卡号
            public string Phone { get; set; }//手机号
            public string BankCard { get; set; }//银行卡号
        }
        //建立“车位信息”实体类PackingPlace
        public class PackingPlace
        {
            [Key]
        public string PlaceID { get; set; }

        public string Type { get; set; }//类型

        public string PackingCondition { get; set; }//车位状态
        public string LocationCard { get; set; }//定位标签
    }
        public class SecretPlace
        {
            
            public string PlaceID { get; set; }//车位编号

            public string CardID { get; set; }//卡号
            public string IsAllowingRenting { get; set; }//是否允许租赁
            
        }
    public class SecretPayRecord
    {
        [Key]
        public int RecordID { get; set; }//收费记录号，数值型
        public DateTime RecordDate { get; set; } //记录日期，日期型
        public string CardID { get; set; }
        public decimal Revenue { get; set; }
    }

    public class PayRecord
    {
        [Key]
        public int RecordID { get; set; }//收费记录号，数值型
        public DateTime RecordDate { get; set; } //记录日期，日期型
        public string CardID { get; set; }
        public decimal Pay { get; set; }
        public decimal Revenue { get; set; }
        public string PayCondition { get; set; }
    }
    public class RentingTime
        {

            public string PlaceID { get; set; }//车位编号

            public float StartTime1 { get; set; }//开始时间1
            public float StopTime1 { get; set; }//结束时间1
            public float StartTime2 { get; set; }//开始时间2
            public float StopTime2 { get; set; }//结束时间2
            public float StartTime3 { get; set; }//开始时间3
            public float StopTime3 { get; set; }//结束时间3

        }
        //下面2个实体，by李畅
        //建立“公共车位”实体类CommonMess，操作的表是PackingPlace
        public class CommonMess
        {
            [Key]
            public string PlaceID { get; set; }//停车位，字符串型，主键 S（A）代表私有车位，C（B）代表公共车位，可以，可为空
            public string PackingCondition { get; set; }//车位状态，即是否停有车，字符串型，值域为（packed，unpacked ）
        }

        //公共普通用户收费管理,操作的表是CommonPayRecord
        public class CommonPayRecord
        {
            [Key]
            public int RecordID { get; set; }//收费记录号，数值型
            public DateTime RecordDate { get; set; } //记录日期，日期型，与id共同构成主键
            public string Type { get; set; }//租用的车位类型，字符串型，值域为（secret，common）
            public DateTime InTime { get; set; }//进入停车场时间，日期时间型
            public DateTime OutTime { get; set; }//出停车场时间，日期时间型，出场时间晚于入场时间
            public decimal Revenue { get; set; }//用户所付的费用，金钱型
            public string CardID { get; set; }//卡号，字符串型，主键，首字母为C(B)
            public string PlaceID { get; set; }//停车位，字符串型，S（A）代表私有车位，C（B）代表公共车位，可以，可为空
        }
    public class SecretCard//固定用户停车卡
    {

        public string ID { get; set; }

        public string PlaceID { get; set; }

        public DateTime ValidDate { get; set; }

        public string RentingCondition { get; set; }
        public string ValidCondition { get; set; }
        public float Revenue { get; set; }
        public float Pay { get; set; }
    }
}
