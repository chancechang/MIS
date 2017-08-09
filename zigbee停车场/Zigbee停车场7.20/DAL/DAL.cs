using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity;
using System.Reflection;

namespace DAL
{

    public class CommonPayRecordDAL
    {
        //连接数据库
        public static SqlConnection sqlCon1 = new SqlConnection(@"Data Source=192.168.0.254;Initial Catalog=Packing;
                                                      Integrated Security=False;User ID=sa;Password=abc123@");
        public static List<CommonPayRecord> QueryListByCondition()
        {

            //要执行的SQL语句
            Model.CommonPayRecord model = null;
            StringBuilder sbSql = new StringBuilder("select * from CommonPayRecord");
            //读取数据库 返回查询到的数据表(如果参数为null,则直接执行sql语句，否则带参数执行sql语句)
            SqlCommand cmdname = new SqlCommand(sbSql.ToString(), sqlCon1);
            //SqlDataAdapter是 DataSet和 SQL Server之间的桥接器
            SqlDataAdapter da = new SqlDataAdapter(cmdname);//SqlCommand是sql命令,执行后通过sqlDataAdapter返回填入DataSet,
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            //准备要返回的泛型集合
            List<Model.CommonPayRecord> list = null;
            if (dt.Rows.Count > 0)//如果查询到的行数大于0
            {
                list = new List<Model.CommonPayRecord>();//实例化集合对象
                foreach (DataRow dr in dt.Rows)//循环临时表的行记录
                {
                    model = new Model.CommonPayRecord();//每循环一行生成一个实体
                    SetDr2Model(dr, model);//将行数据填入实体对应的属性
                    list.Add(model);//将实体对象加入集合
                }
            }
            return list;
        }
        //添加停车信息
        public static void addCommonPayRecord(string cardid, string placeid,string type)
        {
         
            string MyInsert = "insert into CommonPayRecord(CardID,PlaceID,Type)values('" + cardid + "','" + placeid + "','" + type + "')";
            SqlCommand MyCommand = new SqlCommand(MyInsert, sqlCon1);
            try//异常处理  
            {
                sqlCon1.Open();
                MyCommand.ExecuteNonQuery();
                sqlCon1.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught.", ex);
            }
        }
        public static DataSet QueryCommonPay()
        {

            //要执行的SQL语句
            Model.SecretPayRecord model = null;
            StringBuilder sbSql = new StringBuilder("select recordID as 记录号,recorddate as 记录日期,cardid as 卡号,placeid as 车位号,type as 租用车位类型,revenue as 支付金额,intime as 入库时间,outtime as 出库时间 from CommonPayRecord");
            //读取数据库 返回查询到的数据表(如果参数为null,则直接执行sql语句，否则带参数执行sql语句)
            SqlCommand cmdname = new SqlCommand(sbSql.ToString(), sqlCon1);
            //SqlDataAdapter是 DataSet和 SQL Server之间的桥接器
            SqlDataAdapter da = new SqlDataAdapter(cmdname);//SqlCommand是sql命令,执行后通过sqlDataAdapter返回填入DataSet,
            DataSet ds = new DataSet();
            da.Fill(ds);
            //DataTable dt = ds.Tables[0];
            //准备要返回的泛型集合

            return ds;
        }
        public static DataSet QuerySecretPay()
        {

            //要执行的SQL语句
            Model.SecretPayRecord model = null;
            StringBuilder sbSql = new StringBuilder("select recordID as 记录号,recorddate as 记录日期,cardid as 卡号,revenue as 支付金额 from SecretPayRecord");
            //读取数据库 返回查询到的数据表(如果参数为null,则直接执行sql语句，否则带参数执行sql语句)
            SqlCommand cmdname = new SqlCommand(sbSql.ToString(), sqlCon1);
            //SqlDataAdapter是 DataSet和 SQL Server之间的桥接器
            SqlDataAdapter da = new SqlDataAdapter(cmdname);//SqlCommand是sql命令,执行后通过sqlDataAdapter返回填入DataSet,
            DataSet ds = new DataSet();
            da.Fill(ds);
            //DataTable dt = ds.Tables[0];
            //准备要返回的泛型集合

            return ds;
        }

        public static DataSet analyse(string s)
        {

            //要执行的SQL语句
            Model.SecretPayRecord model = null;
            String sbSql = null;
            if (s == "day")
            {
                string usetime = "sum(datediff(minute,intime,outtime))";
                sbSql = "select year(recorddate) as 年,month(recorddate) as 月,day(recorddate) as 日,placeid as 停车位," + usetime + " as 使用时间,1440 as 总时间,round(convert(float," + usetime + ")/convert(float,1440),2) as 使用率 from CommonPayRecord where type='common' group by placeid,year(recorddate),month(recorddate),day(recorddate)";
            }
            else if (s == "month")
            {
                string usetime = "sum(datediff(hour,intime,outtime))";
                string sumtime = "sum(datediff(hour,dateadd(dd,-day(recorddate)+1,recorddate),dateadd(dd,-day(recorddate),dateadd(m,1,recorddate))))"; //每月的总小时数
                sbSql = "select year(recorddate) as 年,month(recorddate) as 月,placeid as 停车位," + usetime + "as 使用时间," + sumtime + "as 总时间,round(convert(float," + usetime + ")/convert(float," + sumtime + "),2) as 使用率 from CommonPayRecord where type='common' group by placeid,year(recorddate),month(recorddate)";
            }
            else if (s == "year")
            {
                string usetime = "sum(datediff(hour,intime,outtime))";
                string sumtime = "sum(datediff(hour,DATEADD(yy, DATEDIFF(yy,0,getdate()), 0),DATEADD(DD,-1,DATEADD(YY,1,DATEADD(YY,DATEDIFF(YY,0,GETDATE()),0)))))"; //每nian的总小时数
                sbSql = "select year(recorddate) as 年,placeid as 停车位," + usetime + "as 使用时间," + sumtime + "as 总时间,round(convert(float," + usetime + ")/convert(float," + sumtime + "),2) as 使用率 from CommonPayRecord where type='common' group by placeid,year(recorddate)";
            }
            else
            {

            }
            //读取数据库 返回查询到的数据表(如果参数为null,则直接执行sql语句，否则带参数执行sql语句)
            SqlCommand cmdname = new SqlCommand(sbSql.ToString(), sqlCon1);
            //SqlDataAdapter是 DataSet和 SQL Server之间的桥接器
            SqlDataAdapter da = new SqlDataAdapter(cmdname);//SqlCommand是sql命令,执行后通过sqlDataAdapter返回填入DataSet,
            DataSet ds = new DataSet();
            da.Fill(ds);
            //DataTable dt = ds.Tables[0];
            //准备要返回的泛型集合
            return ds;
        }

        public static DataSet QueryPay()
        {

            //要执行的SQL语句
            Model.PayRecord model = null;
            StringBuilder sbSql = new StringBuilder("select recordid as 记录号,recorddate as 记录日期,cardid as 卡号,revenue as 租用收入,pay as 租用应付,paycondition as 支付状态 from PayRecord");
            //读取数据库 返回查询到的数据表(如果参数为null,则直接执行sql语句，否则带参数执行sql语句)
            SqlCommand cmdname = new SqlCommand(sbSql.ToString(), sqlCon1);
            //SqlDataAdapter是 DataSet和 SQL Server之间的桥接器
            SqlDataAdapter da = new SqlDataAdapter(cmdname);//SqlCommand是sql命令,执行后通过sqlDataAdapter返回填入DataSet,
            DataSet ds = new DataSet();
            da.Fill(ds);
            //DataTable dt = ds.Tables[0];
            //准备要返回的泛型集合

            return ds;
        }        

        public static DataTable Report(string s)
        {
            string sqlquery0 = null, sqlquery1 = null, sqlquery2 = null, sqlquery = null;
            if (s == "day")
            {

                sqlquery0 = "select year(recorddate) as Year,month(recorddate) as Month,day(recorddate) as Day,sum(Pay) as PayForSecret,sum(Revenue) as RevenueForSecret from PayRecord group by day(recorddate),month(recorddate),year(recorddate) ";
                sqlquery1 = "select year(recorddate) as Year,month(recorddate) as Month,day(recorddate) as Day,sum(Revenue) as SecretRevenue from SecretPayRecord  group by day(recorddate),month(recorddate),year(recorddate) ";
                sqlquery2 = "select year(recorddate) as Year,month(recorddate) as Month,day(recorddate) as Day,sum(Revenue) as CommonRevenue from CommonPayRecord  group by day(recorddate),month(recorddate),year(recorddate) ";
                sqlquery = "select t1.Year as 年,t1.Month as 月,t1.Day as 日,t1.PayForSecret as 租用应付,t1.RevenueForSecret as 租用收入,t2.SecretRevenue as 固定用户付费金额,t3.CommonRevenue as 公共用户付费金额 from (" + sqlquery0 + ") as t1 left join (" + sqlquery1 + ") as t2 on t1.Year=t2.Year and t1.Month=t2.Month and t1.Day=t2.Day  left join (" + sqlquery2 + ") as t3 on t3.Year=t2.Year and t3.Month=t2.Month and t3.Day=t2.Day";
            }
             else if (s == "month")
            {
                sqlquery0 = "select year(recorddate) as Year,month(recorddate) as Month,sum(Pay) as PayForSecret,sum(Revenue) as RevenueForSecret from PayRecord group by month(recorddate),year(recorddate) ";
                sqlquery1 = "select year(recorddate) as Year,month(recorddate) as Month,sum(Revenue) as SecretRevenue from SecretPayRecord  group by month(recorddate),year(recorddate) ";
                sqlquery2 = "select year(recorddate) as Year,month(recorddate) as Month,sum(Revenue) as CommonRevenue from CommonPayRecord  group by month(recorddate),year(recorddate) ";
                sqlquery = "select t1.Year as 年,t1.Month as 月,t1.PayForSecret as 租用应付,t1.RevenueForSecret as 租用收入,t2.SecretRevenue as 固定用户付费金额,t3.CommonRevenue as 公共用户付费金额 from (" + sqlquery0 + ") as t1 left join (" + sqlquery1 + ") as t2 on t1.Year=t2.Year and t1.Month=t2.Month   left join (" + sqlquery2 + ") as t3 on t3.Year=t2.Year and t3.Month=t2.Month";
            }
            else if (s == "year")
            {
                sqlquery0 = "select year(recorddate) as Year,sum(Pay) as PayForSecret,sum(Revenue) as RevenueForSecret from PayRecord group by year(recorddate) ";
                sqlquery1 = "select year(recorddate) as Year,sum(Revenue) as SecretRevenue from SecretPayRecord  group by year(recorddate) ";
                sqlquery2 = "select year(recorddate) as Year,sum(Revenue) as CommonRevenue from CommonPayRecord  group by year(recorddate) ";
                sqlquery = "select t1.Year as 年,t1.PayForSecret as 租用应付,t1.RevenueForSecret as 租用收入,t2.SecretRevenue as 固定用户付费金额,t3.CommonRevenue as 公共用户付费金额 from (" + sqlquery0 + ") as t1 left join (" + sqlquery1 + ") as t2 on t1.Year=t2.Year left join (" + sqlquery2 + ") as t3 on t3.Year=t2.Year";
            }
            SqlCommand cmdname = new SqlCommand(sqlquery, sqlCon1);
           
            SqlDataAdapter da = new SqlDataAdapter(cmdname);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            return dt;       
        }

        //通过CardID写入InTime,OutTime
        public static void insertTimebyCardID(string cardid)
        {
            CommonPayRecord model = queryByCardID(cardid);

            if (model != null)
            {
                //查询记录,输出最新一条queryByCardID(string cardid),判断InTime,OutTime
                //判断InTime是否为空，为空则写入，不为空则判断OutTime,为空则写入，不为空则不进行任何操作
                // Console.Write("model.InTime.ToString()" + model.InTime.ToString()+ "model.InTime.ToString()");
                DateTime time = model.InTime;
                if (model.InTime.ToString() == "0001/1/1 0:00:00")
                {
                    string MyInsert = "update CommonPayRecord  set InTime='" + DateTime.Now.ToString() + "' where RecordID='" + model.RecordID + "'";
                    SqlCommand MyCommand = new SqlCommand(MyInsert, sqlCon1);
                    try//异常处理  
                    {
                        sqlCon1.Open();
                        MyCommand.ExecuteNonQuery();
                        sqlCon1.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("{0} Exception caught.", ex);
                    }

                }
                else if ((time.Day.ToString() == DateTime.Now.Day.ToString()) && (time.Hour == DateTime.Now.Hour) && (DateTime.Now.Minute - time.Minute < 2))
                {

                }
                else if (model.OutTime.ToString() == "0001/1/1 0:00:00")
                {
                    string MyInsert = "update CommonPayRecord  set OutTime='" + DateTime.Now.ToString() + "' where RecordID='" + model.RecordID + "'";
                    SqlCommand MyCommand = new SqlCommand(MyInsert, sqlCon1);
                    try//异常处理  
                    {
                        sqlCon1.Open();
                        MyCommand.ExecuteNonQuery();
                        sqlCon1.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("{0} Exception caught.", ex);
                    }

                }
                else
                {

                }
            }
        }



        public static CommonPayRecord queryByCardID(string cardid)
        {
            //查询记录,输出最新一条
            //要执行的SQL语句
            Model.CommonPayRecord model = null;
            StringBuilder sbSql = new StringBuilder("select * from CommonPayRecord where CardID='" + cardid.ToString().Trim() + "'");
            //读取数据库 返回查询到的数据表(如果参数为null,则直接执行sql语句，否则带参数执行sql语句)
            SqlCommand cmdname = new SqlCommand(sbSql.ToString(), sqlCon1);
            //SqlDataAdapter是 DataSet和 SQL Server之间的桥接器
            SqlDataAdapter da = new SqlDataAdapter(cmdname);//SqlCommand是sql命令,执行后通过sqlDataAdapter返回填入DataSet,
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            model = new Model.CommonPayRecord();//每循环一行生成一个实体

            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                DataRow dr = dt.Rows[dt.Rows.Count - 1];
                SetDr2Model(dr, model);
            }
            return model;


        }
        public static string queryCardIDByPlaceID(string placeid)
        {
            //查询记录,输出最新一条
            //要执行的SQL语句
            Model.CommonPayRecord model = null;
            StringBuilder sbSql = new StringBuilder("select * from CommonPayRecord where PlaceID='" + placeid.ToString().Trim() + "'");
            //读取数据库 返回查询到的数据表(如果参数为null,则直接执行sql语句，否则带参数执行sql语句)
            SqlCommand cmdname = new SqlCommand(sbSql.ToString(), sqlCon1);
            //SqlDataAdapter是 DataSet和 SQL Server之间的桥接器
            SqlDataAdapter da = new SqlDataAdapter(cmdname);//SqlCommand是sql命令,执行后通过sqlDataAdapter返回填入DataSet,
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            DataRow dr = dt.Rows[dt.Rows.Count - 1];
            model = new Model.CommonPayRecord();//每循环一行生成一个实体
            SetDr2Model(dr, model);//将行数据填入实体对应的属性           
            return model.CardID;
        }

        public static void insertRevenuebyRecordID(int recordid, int revenue)
        {
            string MyInsert = "update CommonPayRecord  set Revenue='" + revenue.ToString() + "' where RecordID='" + recordid + "'";
            SqlCommand MyCommand = new SqlCommand(MyInsert, sqlCon1);
            try//异常处理  
            {
                sqlCon1.Open();
                MyCommand.ExecuteNonQuery();
                sqlCon1.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught.", ex);
            }
        }
        //将 数据行 转换 成 实体对象
        /// <summary>
        /// 将 数据行 转换 成 实体对象，用于将dataset 的每一行转成实体对象
        /// </summary>
        /// <param name="dr">数据行</param>
        /// <param name="model">实体对象</param>
        ///  
        public static void SetDr2Model(DataRow dr, Model.CommonPayRecord model)
        {
            //收费记录号，数值型
            if (dr["RecordID"].ToString() != "")
            {
                model.RecordID = int.Parse(dr["RecordID"].ToString());
            }
            //记录日期，日期型，与id共同构成主键
            if (dr["RecordDate"].ToString() != "")
            {
                model.RecordDate = Convert.ToDateTime(dr["RecordDate"].ToString());
            }
            //进入停车场时间，日期时间型
            if (dr["InTime"].ToString() != "")
            {
                model.InTime = Convert.ToDateTime(dr["InTime"].ToString());
            }
            //出停车场时间，日期时间型，出场时间晚于入场时间
            if (dr["OutTime"].ToString() != "")
            {
                model.OutTime = Convert.ToDateTime(dr["OutTime"].ToString());
            }
            //用户所付的费用，金钱型
            if (dr["Revenue"].ToString() != "")
            {
                model.Revenue = decimal.Parse(dr["Revenue"].ToString());
            }
            if (dr["Type"].ToString() != "")
            {
                model.Type = dr["Type"].ToString();
            }
            //卡号，字符串型，主键，首字母为C
            if (dr["CardID"].ToString() != "")
            {
                model.CardID = dr["CardID"].ToString();
            }
            //停车位，字符串型，S代表私有车位，C代表公共车位
            if (dr["PlaceID"].ToString() != "")
            {
                model.PlaceID = dr["PlaceID"].ToString();
            }
        }
    }

    
    //公共车位信息查询
    public class CommonMessDAL
    {
        //连接数据库
        public static SqlConnection sqlCon1 = new SqlConnection(@"Data Source=192.168.0.254;Initial Catalog=Packing;
                                                      Integrated Security=False;User ID=sa;Password=abc123@");
        public static List<CommonMess> QueryListByCondition()
        {

            //要执行的SQL语句
            Model.CommonMess model = null;
            StringBuilder sbSql = new StringBuilder("select * from PackingPlace where  Type='" + "common'");
            //读取数据库 返回查询到的数据表(如果参数为null,则直接执行sql语句，否则带参数执行sql语句)
            SqlCommand cmdname = new SqlCommand(sbSql.ToString(), sqlCon1);
            //SqlDataAdapter是 DataSet和 SQL Server之间的桥接器
            SqlDataAdapter da = new SqlDataAdapter(cmdname);//SqlCommand是sql命令,执行后通过sqlDataAdapter返回填入DataSet,
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            //准备要返回的泛型集合
            List<Model.CommonMess> list = null;
            if (dt.Rows.Count > 0)//如果查询到的行数大于0
            {
                list = new List<Model.CommonMess>();//实例化集合对象
                foreach (DataRow dr in dt.Rows)//循环临时表的行记录
                {
                    model = new Model.CommonMess();//每循环一行生成一个实体
                    SetDr2Model(dr, model);//将行数据填入实体对应的属性
                    list.Add(model);//将实体对象加入集合
                }
            }
            return list;
        }
        //对车位是否空闲进行修改
        public static void change(string placeid, string packingcondition)
        {

            string MyUpdate = "Update PackingPlace set PackingCondition ='" + packingcondition + "'where PlaceID='" + placeid + "'";
            SqlCommand MyCommand = new SqlCommand(MyUpdate, sqlCon1);
            try
            {
                sqlCon1.Open();
                MyCommand.ExecuteNonQuery();
                sqlCon1.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught.", ex);
            }
        }
        //根据placeID查询车位是否空闲
        public static string QueryFreebyPlaceID(string placeid)
        {
            Model.CommonMess model = null;
            StringBuilder sbSql = new StringBuilder("select * from PackingPlace where PlaceID='" + placeid + "'");
            //读取数据库 返回查询到的数据表(如果参数为null,则直接执行sql语句，否则带参数执行sql语句)
            SqlCommand cmdname = new SqlCommand(sbSql.ToString(), sqlCon1);
            //SqlDataAdapter是 DataSet和 SQL Server之间的桥接器
            SqlDataAdapter da = new SqlDataAdapter(cmdname);//SqlCommand是sql命令,执行后通过sqlDataAdapter返回填入DataSet,
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            DataRow dr = dt.Rows[0];
            model = new Model.CommonMess();//每循环一行生成一个实体
            SetDr2Model(dr, model);//将行数据填入实体对应的属性           
            return model.PackingCondition;
        }
        //查询公共空闲车位
        public static List<CommonMess> QueryListByFree()
        {

            //要执行的SQL语句
            Model.CommonMess model = null;
            StringBuilder sbSql = new StringBuilder("select * from PackingPlace where  Type='" + "common' and PackingCondition='" + "unpacked'");
            //读取数据库 返回查询到的数据表(如果参数为null,则直接执行sql语句，否则带参数执行sql语句)
            SqlCommand cmdname = new SqlCommand(sbSql.ToString(), sqlCon1);
            //SqlDataAdapter是 DataSet和 SQL Server之间的桥接器
            SqlDataAdapter da = new SqlDataAdapter(cmdname);//SqlCommand是sql命令,执行后通过sqlDataAdapter返回填入DataSet,
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            //准备要返回的泛型集合
            List<Model.CommonMess> list = null;
            if (dt.Rows.Count > 0)//如果查询到的行数大于0
            {
                list = new List<Model.CommonMess>();//实例化集合对象
                foreach (DataRow dr in dt.Rows)//循环临时表的行记录
                {
                    model = new Model.CommonMess();//每循环一行生成一个实体
                    SetDr2Model(dr, model);//将行数据填入实体对应的属性
                    list.Add(model);//将实体对象加入集合
                }
            }
            return list;
        }

        //将 数据行 转换 成 实体对象
        /// <summary>
        /// 将 数据行 转换 成 实体对象，用于将dataset 的每一行转成实体对象
        /// </summary>
        /// <param name="dr">数据行</param>
        /// <param name="model">实体对象</param>
        ///  
        public static void SetDr2Model(DataRow dr, Model.CommonMess model)
        {
            if (dr["PlaceID"].ToString() != "")
            {
                model.PlaceID = dr["PlaceID"].ToString();
            }

            if (dr["PackingCondition"].ToString() != "")
            {
                model.PackingCondition = dr["PackingCondition"].ToString();
            }


        }
    }

    public class SQLDB
    {

        private SqlConnection conn;//创建一个sql数据库打开的连接，参数是连接字符串


        // 数据库连接对象属性      
        public SqlConnection Conn//封装为属性
        {
            get
            {

                string connectionString = @"Data Source=192.168.0.254;Initial Catalog=Packing;
                                                      Integrated Security=False;User ID=sa;Password=abc123@";
                if (conn == null)
                {
                    conn = new SqlConnection(connectionString);
                    conn.Open();
                }
                else if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                else if (conn.State == System.Data.ConnectionState.Broken)
                {
                    conn.Close();
                    conn.Open();
                }
                return conn;
            }
        }

        //关闭数据库连接
        public void CloseDB()
        {
            if (conn.State == System.Data.ConnectionState.Open || conn.State == System.Data.ConnectionState.Broken)
            {
                conn.Close();
            }
        }

        // 创建sqlDataReader对象（每次向数据库只读一条）必须调用 SqlCommand 对象的 ExecuteReader 方法，read（）方法读取 ,每次读一条数据 ，查询 语句
        public SqlDataReader ExecuteReader(string sql, params SqlParameter[] parmaeters)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            if (parmaeters != null)
            {

                cmd.Parameters.AddRange(parmaeters);
            }
            //    conn.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);//返回sqlDataReader对象

        }

        // 执行sql语句 返回数据表  ，查询 ,"safesql"sql语句
        public DataTable GetDataTable(string safeSql)
        {

            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand(safeSql, Conn);

            //SqlDataAdapter是 DataSet和 SQL Server之间的桥接器

            SqlDataAdapter da = new SqlDataAdapter(cmd);//SqlCommand是sql命令,执行后通过sqlDataAdapter返回填入DataSet,
            da.Fill(ds);
            return ds.Tables[0];//将dataset的第一张表返回也就是datatable ,因为dataset是数据表集合 ，相当于内存中的数据库
        }

        // 执行sql语句 返回数据表  ， 查询语句  ,"sql"为sql语句,"values"为sql参数列表
        public DataTable GetDataTable(string sql, params SqlParameter[] values)
        {

            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand(sql, Conn);
            cmd.Parameters.AddRange(values);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            cmd.Parameters.Clear();
            return ds.Tables[0];
        }

        //执行Command  ，  执行增删改 ，"sql"为sql语句，"values"为sql参数数组
        public int ExecuteCommand(string sql, params SqlParameter[] values)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            cmd.Parameters.AddRange(values);
            int result = cmd.ExecuteNonQuery();   //执行(增删改)的方法，返回执行命令所影响的行数（return int类型）
            cmd.Parameters.Clear();
            return result;
        }

        //执行带sql参数的语句，"sql"为sql语句，"values"为sql参数列表
        public int GetScalar(string sql, params SqlParameter[] values)
        {
            object obj = null;
            int i;
            try
            {
                SqlCommand cmd = new SqlCommand(sql, Conn);
                cmd.Parameters.AddRange(values);
                //obj = cmd.ExecuteScalar();//获得查询到的结果集的第一个单元格的值，为obj类型
                i = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseDB();
            }
            if (i == 0)
                return 0;
            else
                //return Convert.ToInt32(obj
                return 1;
        }

        public string GetQueryValue(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string values = reader[0].ToString();
                CloseDB();
                return values;
            }
            else
            {
                CloseDB();
                return null;
            }
        }

    }



    public class AdminDAL//管理员相关操作
    {
        public static int adminlogin;//判断登录用户为超级管理员还是系统管理员
        public static string LoginName;
        public static string ReturnName()
        {
            return LoginName;
        }


        public static int CheckUser()
        {
            if (adminlogin == 1)//如果当前登录系统的为超级管理员，返回1
                return 1;
            else
                return 0;//否则返回0
        }

        // 根据查询条件 返回 管理员实体 列表  ，查询 
        public static List<Administrator> AdminListByCondition(SqlParameter[] paras)
        {
            Model.Administrator model = null;
            SQLDB sqlDB = new SQLDB();
            StringBuilder sbSql = new StringBuilder("select ID as 编号,Name as 姓名,Password as 密码,Phone as 联系方式,Right1 as 权限 from Administrator ");
            if (paras != null)//如果参数数组不为空，则循环生成sql的条件语句，追加查询条件
            {
                for (int i = 0; i < paras.Length; i++)//循环所有参数(如: and PWD=@SPWD)
                {
                    SqlParameter p = paras[i];
                    sbSql.Append(" where ");//第二个参数开始 在前面加 and
                    sbSql.Append(p.ParameterName.Substring(1));//获得参数所对应的列名
                    sbSql.Append("=" + p.ParameterName);
                }
            }
            //读取数据库 返回查询到的数据表(如果参数为null,则直接执行sql语句，否则带参数执行sql语句)
            DataTable dt;
            if (paras == null)
                dt = sqlDB.GetDataTable(sbSql.ToString());
            else
                dt = sqlDB.GetDataTable(sbSql.ToString(), paras);
            //准备要返回的泛型集合
            List<Model.Administrator> list = null;
            if (dt.Rows.Count > 0)//如果查询到的行数大于0
            {
                list = new List<Model.Administrator>();//实例化集合对象
                foreach (DataRow dr in dt.Rows)//循环临时表的行记录 
                {
                    model = new Model.Administrator();//每循环一行生成一个实体
                    SetDr2Model(dr, model);//将行数据填入实体对应的属性
                    list.Add(model);//将实体对象加入集合
                }
            }
            return list;
        }

        //新增系统管理员
        public static int ADD(Model.Administrator MOD)
        {
            SQLDB sqlDB = new SQLDB();
            string sql = "insert into Administrator (ID,Name,Password,Phone,Right1) values (@ID,@Name,@Password,@Phone,@Right1);";
            //insert into 后获得自动插入的id（select @@identity）
            SqlParameter[] pars ={
                                    new SqlParameter("@ID",MOD.ID),
                                    new SqlParameter("@Name",MOD.Name),
                                    new SqlParameter("@Password",MOD.Password),
                                    new SqlParameter("@Phone",MOD.Phone),
                                    new SqlParameter("@Right1",MOD.Right1)

                               };
            int res = sqlDB.GetScalar(sql, pars);  //获得插入id
            return res;
        }
        #region 根据id删除指定行
        public static int DelById(string Id)
        {
            SQLDB sqlDB = new SQLDB();
            int res = sqlDB.ExecuteCommand("delete [Administrator] where Id=@Id", new SqlParameter("@Id", Id));
            return res;
        }
        #endregion

        //修改信息

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public static bool Modify(Model.Administrator MOD)
        {
            SQLDB sqlDB = new SQLDB();
            StringBuilder sbSql = new StringBuilder("update [Administrator] set ");// 与系统关键字冲突  用[] 括起来
            Type modeType = MOD.GetType();//获得对象的类型
            PropertyInfo[] pros = modeType.GetProperties(); // 得到类型的所有公共属性
            List<SqlParameter> paras = new List<SqlParameter>();
            foreach (PropertyInfo pi in pros)   //反射获得属性和属性的值
            {
                if (!pi.Name.Equals("ID"))//如果不是主键则追加sql字符串
                {
                    if (pi.GetValue(MOD, null) != null && !pi.GetValue(MOD, null).ToString().Equals(""))//判断属性值是否为空
                    {

                        sbSql.Append(pi.Name + "=@" + pi.Name + ",");//SID=@SID
                        paras.Add(new SqlParameter("@" + pi.Name, pi.GetValue(MOD, null)));

                    }
                }
            }
            string strSql = sbSql.ToString().Trim(','); //去掉两边的,
            strSql += " where ID=@Id";
            paras.Add(new SqlParameter("@Id", MOD.ID));
            return sqlDB.ExecuteCommand(strSql, paras.ToArray()) > 0;   //执行语句
        }


        // 将 数据行 转换 成 实体对象，用于将dataset 的每一行转成实体对象
        public static void SetDr2Model(DataRow dr, Model.Administrator model)
        {
            if (dr["编号"].ToString() != "")
            {
                model.ID = int.Parse(dr["编号"].ToString());
            }
            if (dr["姓名"].ToString() != "")
            {
                model.Name = dr["姓名"].ToString();
            }
            if (dr["联系方式"].ToString() != "")
            {
                model.Phone = dr["联系方式"].ToString();
            }

            model.Password = dr["密码"].ToString();

            if (dr["权限"].ToString() != "")
            {
                model.Right1 = dr["权限"].ToString();
            }
        }
        //登录管理
        public static int Login(string name, String pwd)
        {
            LoginName = name;
            //连接数据库
            SqlConnection sqlCon1 = new SqlConnection(@"Data Source=192.168.0.254;Initial Catalog=Packing;
                                                      Integrated Security=False;User ID=sa;Password=abc123@");
            //要执行的SQL语句
            sqlCon1.Open();
            String st1 = "select Name from Administrator where Name='" + name.Trim() + "'";
            String st2 = "select Password from Administrator where Name='" + name.Trim() + "' and Password='" + pwd.Trim() + "'";
            String st3 = "select Right1 from Administrator where Name='" + name.Trim() + "'";

            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand(st3, sqlCon1);
            object obj = null;
            obj = cmd.ExecuteScalar();
            adminlogin = Convert.ToInt32(obj);

            SqlCommand cmdname = new SqlCommand(st1, sqlCon1);
            SqlDataReader str1 = cmdname.ExecuteReader(CommandBehavior.CloseConnection);
            if (str1.HasRows)
            {
                str1.Close();
                sqlCon1.Open();
                SqlCommand cmdpwd = new SqlCommand(st2, sqlCon1);
                SqlDataReader str2 = cmdpwd.ExecuteReader();
                if (str2.HasRows)
                {
                    return 0;//登录成功
                }
                else
                {
                    return 1;//密码错误
                }
            }
            else
            {
                return 2;//用户名不存在
            }
        }


    }

    //固定用户相关操作
    public class SecretUserDAL
    {
        // 根据查询条件 返回 固定用户实体 列表  ，查询 
        public static List<SecretUser> SecretUListByCondition(SqlParameter[] paras)
        {
            Model.SecretUser model = null;
            SQLDB sqlDB = new SQLDB();
            StringBuilder sbSql = new StringBuilder("select * from SecretUser ");
            if (paras != null)//如果参数数组不为空，则循环生成sql的条件语句，追加查询条件
            {
                for (int i = 0; i < paras.Length; i++)//循环所有参数(如: and PWD=@SPWD)
                {
                    SqlParameter p = paras[i];
                    sbSql.Append(" where ");//第二个参数开始 在前面加 and
                    sbSql.Append(p.ParameterName.Substring(1));//获得参数所对应的列名
                    sbSql.Append("=" + p.ParameterName);
                }
            }
            //读取数据库 返回查询到的数据表(如果参数为null,则直接执行sql语句，否则带参数执行sql语句)
            DataTable dt;
            if (paras == null)
                dt = sqlDB.GetDataTable(sbSql.ToString());
            else
                dt = sqlDB.GetDataTable(sbSql.ToString(), paras);
            //准备要返回的泛型集合
            List<Model.SecretUser> list = null;
            if (dt.Rows.Count > 0)//如果查询到的行数大于0
            {
                list = new List<Model.SecretUser>();//实例化集合对象
                foreach (DataRow dr in dt.Rows)//循环临时表的行记录 
                {
                    model = new Model.SecretUser();//每循环一行生成一个实体
                    SetDr2Model(dr, model);//将行数据填入实体对应的属性
                    list.Add(model);//将实体对象加入集合
                }
            }
            return list;
        }

        //新增固定用户
        public static int ADDSecretU(Model.SecretUser MOD)
        {
            SQLDB sqlDB = new SQLDB();
            string sql = "insert into SecretUser (Name,Gender,CardID,Phone,BankCard) values (@Name,@Gender,@CardID,@Phone,@BankCard);";
            SqlParameter[] pars ={
                                    //new SqlParameter("@ID",MOD.ID),
                                    new SqlParameter("@Name",MOD.Name),
                                    new SqlParameter("@Gender",MOD.Gender),
                                    new SqlParameter("@CardID",MOD.CardID),
                                    new SqlParameter("@Phone",MOD.Phone),
                                    new SqlParameter("@BankCard",MOD.BankCard)


                               };
            int res = sqlDB.GetScalar(sql, pars);  //获得插入id
            string sql1 = "insert into SecretPayRecord(CardID,Revenue) values ('" + MOD.CardID + "',10000)";
            sqlDB.GetScalar(sql1);
            return res;
        }

        #region 根据id删除指定行
        public static int DelById(string Id)
        {
            SQLDB sqlDB = new SQLDB();
            int res = sqlDB.ExecuteCommand("delete [SecretUser] where Id=@Id", new SqlParameter("@Id", Id));
            return res;
        }
        #endregion

        /// 将 数据行 转换 成 实体对象，用于将dataset 的每一行转成实体对象
        public static void SetDr2Model(DataRow dr, Model.SecretUser model)
        {
            if (dr["ID"].ToString() != "")
            {
                model.ID = int.Parse(dr["ID"].ToString());
            }
            if (dr["Name"].ToString() != "")
            {
                model.Name = dr["Name"].ToString();
            }
            if (dr["Gender"].ToString() != "")
            {
                model.Gender = dr["Gender"].ToString();
            }
            if (dr["CardID"].ToString() != "")
            {
                model.CardID = dr["CardID"].ToString();
            }
            if (dr["Phone"].ToString() != "")
            {
                model.Phone = dr["Phone"].ToString();
            }
            if (dr["BankCard"].ToString() != "")
            {
                model.BankCard = dr["BankCard"].ToString();
            }



        }
        //根据卡号判断固定用户是否在有效期内，固定车位是否已被占用
        public static int SecretUserPack(string CardID,string placeid)
        {
            SQLDB sqlDB = new SQLDB();
            if (placeid != null)
            {
                string sqldb = "select ID from SecretCard where ID ='" + CardID + "'and PlaceID = '" + placeid + "'";
                string check = sqlDB.GetQueryValue(sqldb);
                if (check == null)
                    return 3;
            }
            string sbSql = "select ValidDate from SecretCard where ID ='" + CardID + "'";
            //string sbSql1 = "select RentingCondition from SecretCard where ID ='" + CardID + "'";
            DateTime ValidDate = Convert.ToDateTime(sqlDB.GetQueryValue(sbSql));
            //string RentingCondition = sqlDB.GetQueryValue(sbSql1);
            //Console.WriteLine("baise" + RentingCondition + "huang");
            DateTime Date = DateTime.Now;
            //string r = "unrented  ";
            int i = DateTime.Compare(ValidDate, Date);
            if (i > 0)
            {
                string sql1 = "select PlaceID from SecretCard where ID ='" + CardID + "'";
                string PlaceID = sqlDB.GetQueryValue(sql1);
                string sql2 = "select PackingCondition from PackingPlace where PlaceID ='" + PlaceID + "'";
                string PackingCondition = sqlDB.GetQueryValue(sql2);
                //Console.WriteLine("baise" + PackingCondition + "huang");
                string a = "unpacked  ";
                if (PackingCondition.Equals(a))
                    return 1;
                else
                    return 2;

            }
            else
                return 0;

        }

        //修改车位的PackingCondition
        public static void changePC(string CardID)
        {
            SQLDB sqlDB = new SQLDB();
            string sql1 = "select PlaceID from SecretCard where ID ='" + CardID + "'";
            string PlaceID = sqlDB.GetQueryValue(sql1);
            string sql2 = "update PackingPlace set PackingCondition = 'packed'  where PlaceID ='" + PlaceID + "'";
            sqlDB.ExecuteCommand(sql2);

        }

        //获得位置信息
        public static string Getplaceid(string CardID)
        {
            SQLDB sqlDB = new SQLDB();
            string sql1 = "select PlaceID from SecretCard where ID ='" + CardID + "'";
            string PlaceID = sqlDB.GetQueryValue(sql1);
            return PlaceID;

        }


        //修改固定卡中的收入
        public static void  ModifyRe(string PlaceID,int Revn)
        {
            SQLDB sqlDB = new SQLDB();
            string sql1 = "update  SecretCard set Revenue +='"+ Revn + "'Where PlaceID ='" + PlaceID + "'";
            sqlDB.ExecuteCommand(sql1);


        }
    
        //修改信息

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public static bool Modify(Model.SecretUser MOD)
        {
            SQLDB sqlDB = new SQLDB();
            StringBuilder sbSql = new StringBuilder("update [SecretUser] set ");// 与系统关键字冲突  用[] 括起来
            Type modeType = MOD.GetType();//获得对象的类型
            PropertyInfo[] pros = modeType.GetProperties(); // 得到类型的所有公共属性
            List<SqlParameter> paras = new List<SqlParameter>();
            foreach (PropertyInfo pi in pros)   //反射获得属性和属性的值
            {
                if (!pi.Name.Equals("ID"))//如果不是主键则追加sql字符串
                {
                    if (pi.GetValue(MOD, null) != null && !pi.GetValue(MOD, null).ToString().Equals(""))//判断属性值是否为空
                    {

                        sbSql.Append(pi.Name + "=@" + pi.Name + ",");//SID=@SID
                        paras.Add(new SqlParameter("@" + pi.Name, pi.GetValue(MOD, null)));

                    }
                }
            }
            string strSql = sbSql.ToString().Trim(','); //去掉两边的,
            strSql += " where ID=@Id";
            paras.Add(new SqlParameter("@Id", MOD.ID));
            return sqlDB.ExecuteCommand(strSql, paras.ToArray()) > 0;   //执行语句
        }

        //汽车出库，修改车位状态并清空定位卡号
        public static void ModifyPackingPlace(string CardID)
        {
            SQLDB sqlDB = new SQLDB();
            string sql1 = "select PlaceID from SecretCard where ID ='" + CardID + "'";
            string PlaceID = sqlDB.GetQueryValue(sql1);
            string sql2 = "update PackingPlace set PackingCondition = 'unpacked'  , LocationCard = '' where PlaceID ='" + PlaceID + "'";
            sqlDB.ExecuteCommand(sql2);

        }

    }


    public class SecretPlaceDAL//固定车位相关操作
    {
        public static List<SecretPlace> SecretPListByCondition(SqlParameter[] paras)
        {
            Model.SecretPlace model = null;
            SQLDB sqlDB = new SQLDB();
            StringBuilder sbSql = new StringBuilder("select * from SecretPlace ");
            if (paras != null)//如果参数数组不为空，则循环生成sql的条件语句，追加查询条件
            {
                for (int i = 0; i < paras.Length; i++)//循环所有参数(如: and PWD=@SPWD)
                {
                    SqlParameter p = paras[i];
                    sbSql.Append(" where ");//第二个参数开始 在前面加 and
                    sbSql.Append(p.ParameterName.Substring(1));//获得参数所对应的列名
                    sbSql.Append("=" + p.ParameterName);
                }
            }
            //读取数据库 返回查询到的数据表(如果参数为null,则直接执行sql语句，否则带参数执行sql语句)
            DataTable dt;
            if (paras == null)
                dt = sqlDB.GetDataTable(sbSql.ToString());
            else
                dt = sqlDB.GetDataTable(sbSql.ToString(), paras);
            //准备要返回的泛型集合
            List<Model.SecretPlace> list = null;
            if (dt.Rows.Count > 0)//如果查询到的行数大于0
            {
                list = new List<Model.SecretPlace>();//实例化集合对象
                foreach (DataRow dr in dt.Rows)//循环临时表的行记录
                {
                    model = new Model.SecretPlace();//每循环一行生成一个实体
                    SetDr2Model(dr, model);//将行数据填入实体对应的属性
                    list.Add(model);//将实体对象加入集合
                }
            }
            return list;
        }
        //添加固定车位
        public static int ADDSecretP(Model.SecretPlace MOD)
        {
            SQLDB sqlDB = new SQLDB();
            string sql = "insert into SecretPlace (PlaceID,CardID,IsAllowingRenting) values (@PlaceID,@CardID,@IsAllowingRenting);";
            SqlParameter[] pars ={
                                    new SqlParameter("@PlaceID",MOD.PlaceID),
                                    new SqlParameter("@CardID",MOD.CardID),
                                    new SqlParameter("@IsAllowingRenting",MOD.IsAllowingRenting),
                               };
            int res = sqlDB.GetScalar(sql, pars);  //获得插入id
            return res;
        }
        //删除
        public static int DelById(string Id)
        {
            SQLDB sqlDB = new SQLDB();
            int res = sqlDB.ExecuteCommand("delete [SecretPlace] where PlaceID=@Id", new SqlParameter("@Id", Id));
            return res;
        }
        public static int DelById1(string Id)
        {
            SQLDB sqlDB = new SQLDB();
            int res = sqlDB.ExecuteCommand("delete [SecretPlace] where CardID=@Id", new SqlParameter("@Id", Id));
            return res;
        }

        //修改信息

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public static bool Modify(Model.SecretPlace MOD)
        {
            SQLDB sqlDB = new SQLDB();
            StringBuilder sbSql = new StringBuilder("update [SecretPlace] set ");// 与系统关键字冲突  用[] 括起来
            Type modeType = MOD.GetType();//获得对象的类型
            PropertyInfo[] pros = modeType.GetProperties(); // 得到类型的所有公共属性
            List<SqlParameter> paras = new List<SqlParameter>();
            foreach (PropertyInfo pi in pros)   //反射获得属性和属性的值
            {
                if (!pi.Name.Equals("Id"))//如果不是主键则追加sql字符串
                {
                    if (pi.GetValue(MOD, null) != null && !pi.GetValue(MOD, null).ToString().Equals(""))//判断属性值是否为空
                    {

                        sbSql.Append(pi.Name + "=@" + pi.Name + ",");//SID=@SID
                        paras.Add(new SqlParameter("@" + pi.Name, pi.GetValue(MOD, null)));

                    }
                }
            }
            string strSql = sbSql.ToString().Trim(','); //去掉两边的,
            strSql += " where PlaceID=@Id";
            paras.Add(new SqlParameter("@Id", MOD.PlaceID));
            return sqlDB.ExecuteCommand(strSql, paras.ToArray()) > 0;   //执行语句
        }



        //将 数据行 转换 成 实体对象
        /// <summary>
        /// 将 数据行 转换 成 实体对象，用于将dataset 的每一行转成实体对象
        /// </summary>
        /// <param name="dr">数据行</param>
        /// <param name="model">实体对象</param>
        ///  
        public static void SetDr2Model(DataRow dr, Model.SecretPlace model)
        {
            if (dr["PlaceID"].ToString() != "")
            {
                model.PlaceID = dr["PlaceID"].ToString();
            }
            if (dr["CardID"].ToString() != "")
            {
                model.CardID = dr["CardID"].ToString();
            }
            if (dr["IsAllowingRenting"].ToString() != "")
            {
                model.IsAllowingRenting = dr["IsAllowingRenting"].ToString();
            }

        }

        public static string TemporaryPacking(float time1, float time2)
        {

            SQLDB sqlDB = new SQLDB();
            //string sql1 = "select count( RentingTime.PlaceID ) from RentingTime left join PackingPlace on RentingTime.PlaceID=PackingPlace.PlaceID where  PackingPlace.PackingCondition ='unpacked  '";
            string sql1 = "select count( PlaceID ) from RentingTime";
            int count = Convert.ToInt32(sqlDB.GetQueryValue(sql1));
            for (int i = 0; i < count; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    string sql2 = "select PlaceID from RentingTime where StartTime" + j + "<=" + time1 + " and StopTime" + j + ">=" + time2 + "";
                    string PlaceID = sqlDB.GetQueryValue(sql2);
                    string sql3 = "select PlaceID from PackingPlace where PlaceID = '" + PlaceID + "'and PackingCondition ='unpacked'";
                    PlaceID = sqlDB.GetQueryValue(sql3);
                    if (PlaceID != null)
                    {
                        return PlaceID;
                    }


                }
            }
            return null;//可租用车位表中没有数据

        }

        //传入定位卡卡号LocationCard
        public static void GetLocationCard(string PlaceID, string LocationCard)
        {
            SQLDB sqlDB = new SQLDB();
            string sql2 = "update PackingPlace set LocationCard ='" + LocationCard + "'where PlaceID ='" + PlaceID + "'";
            sqlDB.ExecuteCommand(sql2);

        }
        //修改车位状态
        public static void changePC(string PlaceID)
        {
            SQLDB sqlDB = new SQLDB();
            string sql1 = "update PackingPlace set PackingCondition = 'packed'  where PlaceID ='" + PlaceID + "'";
            sqlDB.ExecuteCommand(sql1);

        }

        //添加用户交易记录
        public static void AddCommonPayRecord(string CardID, string PlaceID)
        {
            SQLDB sqlDB = new SQLDB();
            string sql1 = "insert into CommonPayRecord (Type,CardID,PlaceID) values ('secret','" + CardID + "','" + PlaceID + "')";
            sqlDB.ExecuteCommand(sql1);

        }




    }
    public class PackingPlaceDAL//车位相关操作
    {
        public static List<PackingPlace> PackingPlaceListByCondition(SqlParameter[] paras)
        {
            Model.PackingPlace model = null;
            SQLDB sqlDB = new SQLDB();
            StringBuilder sbSql = new StringBuilder("select * from PackingPlace ");
            if (paras != null)//如果参数数组不为空，则循环生成sql的条件语句，追加查询条件
            {
                for (int i = 0; i < paras.Length; i++)//循环所有参数(如: and PWD=@SPWD)
                {
                    SqlParameter p = paras[i];
                    sbSql.Append(" where ");//第二个参数开始 在前面加 and
                    sbSql.Append(p.ParameterName.Substring(1));//获得参数所对应的列名
                    sbSql.Append("=" + p.ParameterName);
                }
            }
            //读取数据库 返回查询到的数据表(如果参数为null,则直接执行sql语句，否则带参数执行sql语句)
            DataTable dt;
            if (paras == null)
                dt = sqlDB.GetDataTable(sbSql.ToString());
            else
                dt = sqlDB.GetDataTable(sbSql.ToString(), paras);
            //准备要返回的泛型集合
            List<Model.PackingPlace> list = null;
            if (dt.Rows.Count > 0)//如果查询到的行数大于0
            {
                list = new List<Model.PackingPlace>();//实例化集合对象
                foreach (DataRow dr in dt.Rows)//循环临时表的行记录
                {
                    model = new Model.PackingPlace();//每循环一行生成一个实体
                    SetDr2Model(dr, model);//将行数据填入实体对应的属性
                    list.Add(model);//将实体对象加入集合
                }
            }
            return list;
        }
        public static string PackingPlaceListByCondition1(SqlParameter[] paras)
        {
            Model.PackingPlace model = null;
            SQLDB sqlDB = new SQLDB();
            StringBuilder sbSql = new StringBuilder("select * from PackingPlace ");
            if (paras != null)//如果参数数组不为空，则循环生成sql的条件语句，追加查询条件
            {
                for (int i = 0; i < paras.Length; i++)//循环所有参数(如: and PWD=@SPWD)
                {
                    SqlParameter p = paras[i];
                    sbSql.Append(" where ");//第二个参数开始 在前面加 and
                    sbSql.Append(p.ParameterName.Substring(1));//获得参数所对应的列名
                    sbSql.Append("=" + p.ParameterName);
                }
            }
            //读取数据库 返回查询到的数据表(如果参数为null,则直接执行sql语句，否则带参数执行sql语句)
            DataTable dt;
            if (paras == null)
                dt = sqlDB.GetDataTable(sbSql.ToString());
            else
                dt = sqlDB.GetDataTable(sbSql.ToString(), paras);
            //准备要返回的泛型集合
            List<Model.PackingPlace> list = null;
            string a = "";
            if (dt.Rows.Count > 0)//如果查询到的行数大于0
            {
                list = new List<Model.PackingPlace>();//实例化集合对象
                foreach (DataRow dr in dt.Rows)//循环临时表的行记录
                {
                    model = new Model.PackingPlace();//每循环一行生成一个实体
                    SetDr2Model(dr, model);//将行数据填入实体对应的属性
                    a = model.PackingCondition;
                }
            }
            return a.Trim();
        }
        public static string PackingPlaceListByCondition2(SqlParameter[] paras)
        {
            Model.PackingPlace model = null;
            SQLDB sqlDB = new SQLDB();
            StringBuilder sbSql = new StringBuilder("select * from PackingPlace ");
            if (paras != null)//如果参数数组不为空，则循环生成sql的条件语句，追加查询条件
            {
                for (int i = 0; i < paras.Length; i++)//循环所有参数(如: and PWD=@SPWD)
                {
                    SqlParameter p = paras[i];
                    sbSql.Append(" where ");//第二个参数开始 在前面加 and
                    sbSql.Append(p.ParameterName.Substring(1));//获得参数所对应的列名
                    sbSql.Append("=" + p.ParameterName);
                }
            }
            //读取数据库 返回查询到的数据表(如果参数为null,则直接执行sql语句，否则带参数执行sql语句)
            DataTable dt;
            if (paras == null)
                dt = sqlDB.GetDataTable(sbSql.ToString());
            else
                dt = sqlDB.GetDataTable(sbSql.ToString(), paras);
            //准备要返回的泛型集合
            List<Model.PackingPlace> list = null;
            string a = "";
            if (dt.Rows.Count > 0)//如果查询到的行数大于0
            {
                list = new List<Model.PackingPlace>();//实例化集合对象
                foreach (DataRow dr in dt.Rows)//循环临时表的行记录
                {
                    model = new Model.PackingPlace();//每循环一行生成一个实体
                    SetDr2Model(dr, model);//将行数据填入实体对应的属性
                    a = model.Type;
                }
            }
            return a.Trim();
        }
        public static string PackingPlaceListByCondition3(SqlParameter[] paras)
        {
            Model.PackingPlace model = null;
            SQLDB sqlDB = new SQLDB();
            StringBuilder sbSql = new StringBuilder("select * from PackingPlace ");
            if (paras != null)//如果参数数组不为空，则循环生成sql的条件语句，追加查询条件
            {
                for (int i = 0; i < paras.Length; i++)//循环所有参数(如: and PWD=@SPWD)
                {
                    SqlParameter p = paras[i];
                    sbSql.Append(" where ");//第二个参数开始 在前面加 and
                    sbSql.Append(p.ParameterName.Substring(1));//获得参数所对应的列名
                    sbSql.Append("=" + p.ParameterName);
                }
            }
            //读取数据库 返回查询到的数据表(如果参数为null,则直接执行sql语句，否则带参数执行sql语句)
            DataTable dt;
            if (paras == null)
                dt = sqlDB.GetDataTable(sbSql.ToString());
            else
                dt = sqlDB.GetDataTable(sbSql.ToString(), paras);
            //准备要返回的泛型集合
            List<Model.PackingPlace> list = null;
            string a = "";
            if (dt.Rows.Count > 0)//如果查询到的行数大于0
            {
                list = new List<Model.PackingPlace>();//实例化集合对象
                foreach (DataRow dr in dt.Rows)//循环临时表的行记录
                {
                    model = new Model.PackingPlace();//每循环一行生成一个实体
                    SetDr2Model(dr, model);//将行数据填入实体对应的属性
                    a = model.LocationCard;
                }
            }
            return a;
        }

        //添加车位信息
        //public static int ADDPackingP(Model.PackingPlace MOD)
        //{
        //    SQLDB sqlDB = new SQLDB();
        //    string sql = "insert into PackingPlace (PlaceID,Type,HighLocator,HStrength,MediumLocator,MStrength,LowLocator,LStrength,PackingCondition) values (@PlaceID,@Type,@HighLocator,@HStrength,@MediumLocator,@MStrength,@LowLocator,@LStrength,@PackingCondition);";
        //    SqlParameter[] pars ={
        //                            new SqlParameter("@PlaceID",MOD.PlaceID),
        //                            new SqlParameter("@Type",MOD.Type),
        //                            new SqlParameter("@HighLocator",MOD.HighLocator),
        //                            new SqlParameter("@HStrength",MOD.HStrength),
        //                            new SqlParameter("@MediumLocator",MOD.MediumLocator),
        //                            new SqlParameter("@MStrength",MOD.MStrength),
        //                            new SqlParameter("@LowLocator",MOD.LowLocator),
        //                            new SqlParameter("@LStrength",MOD.LStrength),
        //                            new SqlParameter("@PackingCondition",MOD.PackingCondition),
        //                       };
        //    int res = sqlDB.GetScalar(sql, pars);  //获得插入id
        //    return res;
        //}

        //删除
        public static int DelById(string Id)
        {
            SQLDB sqlDB = new SQLDB();
            int res = sqlDB.ExecuteCommand("delete [PackingPlace] where Id=@Id", new SqlParameter("@Id", Id));
            return res;
        }

        //修改信息

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public static bool Modify(Model.PackingPlace MOD)
        {
            SQLDB sqlDB = new SQLDB();
            StringBuilder sbSql = new StringBuilder("update [PackingPlace] set ");// 与系统关键字冲突  用[] 括起来
            Type modeType = MOD.GetType();//获得对象的类型
            PropertyInfo[] pros = modeType.GetProperties(); // 得到类型的所有公共属性
            List<SqlParameter> paras = new List<SqlParameter>();
            foreach (PropertyInfo pi in pros)   //反射获得属性和属性的值
            {
                if (!pi.Name.Equals("PlaceID"))//如果不是主键则追加sql字符串
                {
                    if (pi.GetValue(MOD, null) != null && !pi.GetValue(MOD, null).ToString().Equals(""))//判断属性值是否为空
                    {

                        sbSql.Append(pi.Name + "=@" + pi.Name + ",");//SID=@SID
                        paras.Add(new SqlParameter("@" + pi.Name, pi.GetValue(MOD, null)));

                    }
                }
            }
            string strSql = sbSql.ToString().Trim(','); //去掉两边的,
            strSql += " where PlaceID=@PlaceID";
            paras.Add(new SqlParameter("@PlaceID", MOD.PlaceID));
            return sqlDB.ExecuteCommand(strSql, paras.ToArray()) > 0;   //执行语句
        }



        //将 数据行 转换 成 实体对象
        /// <summary>
        /// 将 数据行 转换 成 实体对象，用于将dataset 的每一行转成实体对象
        /// </summary>
        /// <param name="dr">数据行</param>
        /// <param name="model">实体对象</param>
        ///  
        public static void SetDr2Model(DataRow dr, Model.PackingPlace model)
        {
            if (dr["PlaceID"].ToString() != "")
            {
                model.PlaceID = dr["PlaceID"].ToString();
            }
            if (dr["Type"].ToString() != "")
            {
                model.Type = dr["Type"].ToString();
            }
            if (dr["PackingCondition"].ToString() != "")
            {
                model.PackingCondition = dr["PackingCondition"].ToString();
            }
            if (dr["LocationCard"].ToString() != "")
            {
                model.LocationCard = dr["LocationCard"].ToString();
            }

        }
        //汽车出库，修改车位状态并清空定位卡号
        public static void ModifyPackingPlace(string PlaceID)
        {
            SQLDB sqlDB = new SQLDB();
            string sql1 = "update PackingPlace set PackingCondition = 'unpacked'  , LocationCard = '' where PlaceID ='" + PlaceID + "'";
            sqlDB.ExecuteCommand(sql1);

        }



    }
    public class RentingTimeDAL//可租用信息相关操作
    {
        public static List<RentingTime> RentingTimeListByCondition(SqlParameter[] paras)
        {
            Model.RentingTime model = null;
            SQLDB sqlDB = new SQLDB();
            StringBuilder sbSql = new StringBuilder("select * from RentingTime ");
            if (paras != null)//如果参数数组不为空，则循环生成sql的条件语句，追加查询条件
            {
                for (int i = 0; i < paras.Length; i++)//循环所有参数(如: and PWD=@SPWD)
                {
                    SqlParameter p = paras[i];
                    sbSql.Append(" where ");//第二个参数开始 在前面加 and
                    sbSql.Append(p.ParameterName.Substring(1));//获得参数所对应的列名
                    sbSql.Append("=" + p.ParameterName);
                }
            }
            //读取数据库 返回查询到的数据表(如果参数为null,则直接执行sql语句，否则带参数执行sql语句)
            DataTable dt;
            if (paras == null)
                dt = sqlDB.GetDataTable(sbSql.ToString());
            else
                dt = sqlDB.GetDataTable(sbSql.ToString(), paras);
            //准备要返回的泛型集合
            List<Model.RentingTime> list = null;
            if (dt.Rows.Count > 0)//如果查询到的行数大于0
            {
                list = new List<Model.RentingTime>();//实例化集合对象
                foreach (DataRow dr in dt.Rows)//循环临时表的行记录
                {
                    model = new Model.RentingTime();//每循环一行生成一个实体
                    SetDr2Model(dr, model);//将行数据填入实体对应的属性
                    list.Add(model);//将实体对象加入集合
                }
            }
            return list;
        }

        //添加可租用车位信息
        public static int ADDRentingT(Model.RentingTime MOD)
        {
            SQLDB sqlDB = new SQLDB();
            string sql = "insert into RentingTime (PlaceID,StartTime1,StopTime1,StartTime2,StopTime2,StartTime3,StopTime3) values (@PlaceID,@StartTime1,@StopTime1,@StartTime2,@StopTime2,@StartTime3,@StopTime3);";
            SqlParameter[] pars ={
                                    new SqlParameter("@PlaceID",MOD.PlaceID),
                                    new SqlParameter("@StartTime1",MOD.StartTime1),
                                    new SqlParameter("@StopTime1",MOD.StopTime1),
                                    new SqlParameter("@StartTime2",MOD.StartTime2),
                                    new SqlParameter("@StopTime2",MOD.StopTime2),
                                    new SqlParameter("@StartTime3",MOD.StartTime3),
                                    new SqlParameter("@StopTime3",MOD.StopTime3),
                               };
            int res = sqlDB.GetScalar(sql, pars);  //获得插入id
            return res;
        }
        //删除
        public static int DelById(string Id)
        {
            SQLDB sqlDB = new SQLDB();
            int res = sqlDB.ExecuteCommand("delete [RentingTime] where PlaceID=@Id", new SqlParameter("@Id", Id));
            return res;
        }


        //修改信息

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public static bool Modify(Model.RentingTime MOD)
        {
            SQLDB sqlDB = new SQLDB();
            StringBuilder sbSql = new StringBuilder("update [RentingTime] set ");// 与系统关键字冲突  用[] 括起来
            Type modeType = MOD.GetType();//获得对象的类型
            PropertyInfo[] pros = modeType.GetProperties(); // 得到类型的所有公共属性
            List<SqlParameter> paras = new List<SqlParameter>();
            foreach (PropertyInfo pi in pros)   //反射获得属性和属性的值
            {
                if (!pi.Name.Equals("PlaceID"))//如果不是主键则追加sql字符串
                {
                    if (pi.GetValue(MOD, null) != null && !pi.GetValue(MOD, null).ToString().Equals(""))//判断属性值是否为空
                    {

                        sbSql.Append(pi.Name + "=@" + pi.Name + ",");//SID=@SID
                        paras.Add(new SqlParameter("@" + pi.Name, pi.GetValue(MOD, null)));

                    }
                }
            }
            string strSql = sbSql.ToString().Trim(','); //去掉两边的,
            strSql += " where PlaceID=@PlaceID";
            paras.Add(new SqlParameter("@PlaceID", MOD.PlaceID));
            return sqlDB.ExecuteCommand(strSql, paras.ToArray()) > 0;   //执行语句
        }

        public static string checkPlaceCondition(string type)
        {
            SQLDB sqlDB = new SQLDB();
            string sql = "select count (Type) from PackingPlace where Type = '" + type + "'and PackingCondition = 'unpacked' ";
            string count = sqlDB.GetQueryValue(sql);
            return count;

        }


        //将 数据行 转换 成 实体对象
        /// <summary>
        /// 将 数据行 转换 成 实体对象，用于将dataset 的每一行转成实体对象
        /// </summary>
        /// <param name="dr">数据行</param>
        /// <param name="model">实体对象</param>
        ///  
        public static void SetDr2Model(DataRow dr, Model.RentingTime model)
        {
            if (dr["PlaceID"].ToString() != "")
            {
                model.PlaceID = dr["PlaceID"].ToString();
            }

            if (dr["StartTime1"].ToString() != "")
            {
                model.StartTime1 = float.Parse(dr["StartTime1"].ToString());
            }
            if (dr["StopTime1"].ToString() != "")
            {
                model.StopTime1 = float.Parse(dr["StopTime1"].ToString());
            }

            if (dr["StartTime2"].ToString() != "")
            {
                model.StartTime2 = float.Parse(dr["StartTime2"].ToString());
            }
            if (dr["StopTime2"].ToString() != "")
            {
                model.StopTime2 = float.Parse(dr["StopTime2"].ToString());
            }

            if (dr["StartTime3"].ToString() != "")
            {
                model.StartTime3 = float.Parse(dr["StartTime3"].ToString());
            }
            if (dr["StopTime3"].ToString() != "")
            {
                model.StopTime3 = float.Parse(dr["StopTime3"].ToString());
            }

        }


    }
    public class SecretCardDAL//固定用户停车卡相关操作
    {
        public static List<SecretCard> ListByCondition(SqlParameter[] paras)
        {
            Model.SecretCard model = null;
            SQLDB sqlDB = new SQLDB();
            StringBuilder sbSql = new StringBuilder("select * from SecretCard ");
            if (paras != null)//如果参数数组不为空，则循环生成sql的条件语句，追加查询条件
            {
                for (int i = 0; i < paras.Length; i++)//循环所有参数(如: and PWD=@SPWD)
                {
                    SqlParameter p = paras[i];
                    sbSql.Append(" where ");//第二个参数开始 在前面加 and
                    sbSql.Append(p.ParameterName.Substring(1));//获得参数所对应的列名
                    sbSql.Append("=" + p.ParameterName);
                }
            }
            //读取数据库 返回查询到的数据表(如果参数为null,则直接执行sql语句，否则带参数执行sql语句)
            DataTable dt;
            if (paras == null)
                dt = sqlDB.GetDataTable(sbSql.ToString());
            else
                dt = sqlDB.GetDataTable(sbSql.ToString(), paras);
            //准备要返回的泛型集合
            List<Model.SecretCard> list = null;
            if (dt.Rows.Count > 0)//如果查询到的行数大于0
            {
                list = new List<Model.SecretCard>();//实例化集合对象
                foreach (DataRow dr in dt.Rows)//循环临时表的行记录
                {
                    model = new Model.SecretCard();//每循环一行生成一个实体
                    SetDr2Model(dr, model);//将行数据填入实体对应的属性
                    list.Add(model);//将实体对象加入集合
                }
            }
            return list;
        }


        //添加
        public static int ADD(Model.SecretCard MOD)
        {
            SQLDB sqlDB = new SQLDB();
            string sql = "insert into SecretCard (ID,PlaceID,ValidDate,RentingCondition,ValidCondition,Revenue,Pay) values (@ID,@PlaceID,@ValidDate,@RentingCondition,@ValidCondition,@Revenue,@Pay);";
            SqlParameter[] pars ={
                                        new SqlParameter("@ID",MOD.ID),
                                        new SqlParameter("@PlaceID",MOD.PlaceID),
                                        new SqlParameter("@ValidDate",MOD.ValidDate),
                                        new SqlParameter("@RentingCondition",MOD.RentingCondition),
                                        new SqlParameter("@ValidCondition",MOD.ValidCondition),
                                        new SqlParameter("@Revenue",MOD.Revenue),
                                        new SqlParameter("@Pay",MOD.Pay),
                                   };
            int res = sqlDB.GetScalar(sql, pars);  //获得插入id
            return res;
        }

        //删除
        public static int DelById(string Id)
        {
            SQLDB sqlDB = new SQLDB();
            int res = sqlDB.ExecuteCommand("delete [SecretCard] where Id=@Id", new SqlParameter("@Id", Id));
            return res;
        }

        //修改信息

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        //public static bool Modify(Model.SecretCard MOD)
        //{
        //    SQLDB sqlDB = new SQLDB();
        //    StringBuilder sbSql = new StringBuilder("update [SecretCard] set ");// 与系统关键字冲突  用[] 括起来
        //    Type modeType = MOD.GetType();//获得对象的类型
        //    PropertyInfo[] pros = modeType.GetProperties(); // 得到类型的所有公共属性
        //    List<SqlParameter> paras = new List<SqlParameter>();
        //    foreach (PropertyInfo pi in pros)   //反射获得属性和属性的值
        //    {
        //        if (!pi.Name.Equals("ID"))//如果不是主键则追加sql字符串
        //        {
        //            if (pi.GetValue(MOD, null) != null && !pi.GetValue(MOD, null).ToString().Equals(""))//判断属性值是否为空
        //            {

        //                sbSql.Append(pi.Name + "=@" + pi.Name + ",");//SID=@SID
        //                paras.Add(new SqlParameter("@" + pi.Name, pi.GetValue(MOD, null)));

        //            }
        //        }
        //    }
        //    string strSql = sbSql.ToString().Trim(','); //去掉两边的,
        //    strSql += " where ID=@ID";
        //    paras.Add(new SqlParameter("@ID", MOD.ID));
        //    return sqlDB.ExecuteCommand(strSql, paras.ToArray()) > 0;   //执行语句
        //}



        //将 数据行 转换 成 实体对象
        /// <summary>
        /// 将 数据行 转换 成 实体对象，用于将dataset 的每一行转成实体对象
        /// </summary>
        /// <param name="dr">数据行</param>
        /// <param name="model">实体对象</param>
        ///  
        public static void SetDr2Model(DataRow dr, Model.SecretCard model)
        {
            if (dr["ID"].ToString() != "")
            {
                model.ID = dr["ID"].ToString();
            }
            if (dr["PlaceID"].ToString() != "")
            {
                model.PlaceID = dr["PlaceID"].ToString();
            }
            if (dr["ValidDate"].ToString() != "")
            {
                model.ValidDate = Convert.ToDateTime(dr["ValidDate"].ToString());
            }
            if (dr["RentingCondition"].ToString() != "")
            {
                model.RentingCondition = dr["RentingCondition"].ToString();
            }
            if (dr["ValidCondition"].ToString() != "")
            {
                model.ValidCondition = dr["ValidCondition"].ToString();
            }
            if (dr["Revenue"].ToString() != "")
            {
                model.Revenue = float.Parse(dr["Revenue"].ToString());
            }
            if (dr["Pay"].ToString() != "")
            {
                model.Pay = float.Parse(dr["Pay"].ToString());
            }


        }

    }
}