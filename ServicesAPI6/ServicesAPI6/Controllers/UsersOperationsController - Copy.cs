using DAL.Model;
using EFLibCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ServicesAPI6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersOperationsController : ControllerBase
    {
        List<User> userList = new List<User>();
        Result _result = new Result();
        LoggerCls logger = new LoggerCls();

        /// <summary>
        /// Sistemdeki kullan�c� bilgilerini getirir.
        /// </summary>
        /// <returns> Kullan�c� listesini d�ner </returns>
        [HttpGet]
        public List<User> GetUser()
        {

            userList = AddUser().OrderBy(x => x.Age).ToList();
            return userList;
        }

        /// <summary>
        /// Sistemdeki terk bir kullan�c�y� getirir.
        /// </summary>
        /// <param name="userId">�stenen kullan�c�n�n userId bilgisi</param>
        /// <returns></returns>

        [HttpGet("{id}")]
        public User GetUser(int id)
        {
            List<User> userList = new List<User>();
            userList = AddUser();

            User resultObject = new User();
            resultObject = userList.FirstOrDefault(x => x.Id == id);
            return resultObject;
           
        }

        /// <summary>
      /// Yeni bir kullan�c� ekler
      /// </summary>
      /// <param name="user"></param>
      /// <returns></returns>
        
        [HttpPost]
        public Result Post(User user)
        {
            //liste dolduruluyor.
            userList = AddUser();

            //Yeni eleman listede var m� ? 
            bool userCheck = userList.Select(x => x.Id == user.Id || x.UserName == user.UserName).FirstOrDefault();
            if(userCheck == false)
            {
                //Listeye yeni eleman ekleniyor.
                userList.Add(user);
                _result.status = 1;
                _result.Message = "Yeni eleman listeye eklendi.";
            }
            else
            {
                _result.status = 0;
                _result.Message = "Bu eleman listede zaten var.";
            }
            
            return _result;
        }

        /// <summary>
        /// Kullan�c� g�nceller. 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        /// 
        
        [HttpPut("{UserId}")]
        public Result Update(int UserId, User newValue)
        {
            //liste dolduruluyor.
            userList = AddUser();

            //Kullan�c� g�ncelleme i�lemi yap�l�r.
            User? _oldValue = userList.Find(o => o.Id == UserId);
            if (_oldValue != null)
            {
                userList.Add(newValue);
                userList.Remove(_oldValue);

                _result.status = 1;
                _result.Message = "Kullan�c� bilgileri ba�ar�yla g�ncellendi";
                _result.UserList = userList;
            }
            else
            {
                _result.status = 0;
                _result.Message = "Bu kullan�c�y� i�erde bulamad�k.";
            }
            return _result;

        }
        
        /// <summary>
        /// Kullan�c� sil
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpDelete("{UserId}")]
        public Result Delete(int UserId)
        {
            try
            {
                //liste dolduruluyor.
                userList = AddUser();

                User? _oldValue = userList.Find(o => o.Id == UserId);
                if (_oldValue != null)
                {
                    userList.Remove(_oldValue);
                    _result.status = 1;
                    _result.Message = "Kullan�c� silindi";
                    _result.UserList = userList;
                    logger.createLog(UserId.ToString() + " Id li " + _result.Message);
                }
                else
                {
                    _result.status = 0;
                    _result.Message = "Kullan�c� zaten silinmi�ti.";
                    logger.createLog(UserId.ToString() + " Id li " + _result.Message);
                }
            }
            catch(Exception ex)
            {
                logger.createLog(UserId.ToString() + " Id li " + ex.StackTrace);
            }
            return _result;
        }
    
        public List<User> AddUser()
        {
            List<User> lst = new List<User>();

            lst.Add(new User { Age = 28, Name = "Pelin", UserName = "Pelin28", Id = 1 });
            lst.Add(new User { Age = 35, Name = "Ahmet", UserName = "Demirci08", Id = 2 });
            lst.Add(new User { Age = 25, Name = "Cihat", UserName = "Cihat111", Id = 3 });

            return lst;
        }

       
    }
}