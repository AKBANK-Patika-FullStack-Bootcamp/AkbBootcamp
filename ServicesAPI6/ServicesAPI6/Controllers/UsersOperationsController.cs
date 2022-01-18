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

        DBOperations dbOperation = new DBOperations();
        /// <summary>
        /// Sistemdeki kullanýcý bilgilerini getirir.
        /// </summary>
        /// <returns> Kullanýcý listesini döner </returns>

        [Authorize]
        [HttpGet]
        public List<User> GetUser()
        {
           
            return dbOperation.GetUsers();
        }

        [HttpGet("/UsersOperations/GetUserPaging")]
        public IActionResult GetUserPaging([FromQuery] OwnerParameters ownerParameters)
        {
         var owners = dbOperation.GetUsers() //Db de users tablosundaki herþey
         //.OrderBy(on => on.Name) //Sýralama
        .Skip(ownerParameters.PageNumber) //kaçýncý kayýttn itibaren veri gelecek
        .Take(ownerParameters.PageSize) //Belirttiðimiz sayýda kayýt getirir. Örn : 3, 2 
        .ToList();


            return Ok(owners);
        }

        /// <summary>
        /// Sistemdeki terk bir kullanýcýyý getirir.
        /// </summary>
        /// <param name="userId">Ýstenen kullanýcýnýn userId bilgisi</param>
        /// <returns></returns>

        [HttpGet("{id}")]
        public User GetUser(int id)
        {
            List<User> userList = new List<User>();

            User? resultObject = new User();
            resultObject = userList.Find(x => x.Id == id);
            return resultObject;
           
        }

        /// <summary>
        /// Yeni bir kullanýcý ekler
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        [HttpPost]
        public Result Post(User user)
        {
            User usr = dbOperation.FindUser(user.Name, user.UserName);
            //Yeni eleman listede var mý ? 
            bool userCheck = (usr != null) ?  true : false;
           
            if (userCheck == false)
            {
                //Listeye yeni eleman ekleniyor.
                if(dbOperation.AddModel(user) == true)
                {
                    _result.status = 1;
                    _result.Message = "Yeni eleman listeye eklendi.";
                }
                else
                {
                    _result.status = 0;
                    _result.Message = "Hata, kullanýcý eklenemedi.";
                }
                
            }
            else
            {
                _result.status = 0;
                _result.Message = "Bu eleman listede zaten var.";
            }
            
            return _result;
        }

        /// <summary>
        /// Kullanýcý günceller. 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        /// 
        
        [HttpPut("{UserId}")]
        public Result Update(int UserId, User newValue)
        {

            //Kullanýcý güncelleme iþlemi yapýlýr.
            User? _oldValue = userList.Find(o => o.Id == UserId);
            if (_oldValue != null)
            {
                userList.Add(newValue);
                userList.Remove(_oldValue);

                _result.status = 1;
                _result.Message = "Kullanýcý bilgileri baþarýyla güncellendi";
                _result.UserList = userList;
            }
            else
            {
                _result.status = 0;
                _result.Message = "Bu kullanýcýyý içerde bulamadýk.";
            }
            return _result;

        }
        
        /// <summary>
        /// Kullanýcý sil
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpDelete("{UserId}")]
        public Result Delete(int UserId)
        {
            if (dbOperation.DeleteModel(UserId))
            {
                _result.status = 1;
                _result.Message = "Kullanýcý silindi";
                _result.UserList = userList;
            }
            else
            {
                _result.status = 0;
                _result.Message = "Kullanýcý zaten silinmiþti.";
            }
            return _result;
        }

    }
}