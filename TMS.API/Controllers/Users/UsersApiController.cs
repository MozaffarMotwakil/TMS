using Microsoft.AspNetCore.Mvc;
using TMS.Application.DTOs.Users;
using TMS.Application.Interfaces.Users;

[Route("api/users")]
[ApiController]
public class UsersApiController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersApiController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("AddUser")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDTO>> AddUser([FromBody] UserToAddDTO userToAdd)
    {
        var errors = ValidateUser(userToAdd);

        if (errors.Any())
            return BadRequest(new { Errors = errors });

        var newId = await _userService.AddAsync(userToAdd);
        var created = await _userService.GetByIdAsync(newId);

        if (created == null)
            return StatusCode(500, "حدث خطأ أثناء إنشاء المستخدم");

        return CreatedAtRoute("GetUserById", new { id = newId }, created);
    }

    
    [HttpPut("UpdateUser")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser([FromBody] UserToUpdateDTO dto)
    {
        if (dto == null)
            return BadRequest("البيانات غير صحيحة");

        var errors = ValidateUpdateUser(dto);

        if (errors.Any())
            return BadRequest(new { Errors = errors });

        var result = await _userService.UpdateAsync(dto);

        if (!result)
            return StatusCode(500, "حدث خطأ أثناء تحديث المستخدم");

        return NoContent();
    }

    [HttpDelete("DeleteUser/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (id < 1)
            return BadRequest("المعرف غير صحيح");

        var result = await _userService.DeleteAsync(id);

        if (!result)
            return NotFound("المستخدم غير موجود");

        return NoContent();
    }

    [HttpGet("GetUserById/{id}", Name = "GetUserById")]
     public async Task<ActionResult<UserDTO>> GetByUserId(int id)
    {
        if (id < 1)
            return BadRequest("المعرف غير صحيح");

        var user = await _userService.GetByIdAsync(id);

        if (user == null)
            return NotFound("لم يتم العثور على المستخدم");

        return Ok(user);
    }

    [HttpGet("GetAllUsers")]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
    {
        var result = await _userService.GetAllAsync();
        return Ok(result);
    }

    private List<string> ValidateUpdateUser(UserToUpdateDTO user)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(user.UserName))
            errors.Add("اسم المستخدم مطلوب");

        if (!string.IsNullOrWhiteSpace(user.Email) && !IsValidEmail(user.Email))
            errors.Add("البريد الإلكتروني غير صحيح");

        if (!string.IsNullOrWhiteSpace(user.Phone) && !IsValidPhone(user.Phone))
            errors.Add("رقم الهاتف غير صحيح");

        if (user.DateOfBirth == default)
            errors.Add("تاريخ الميلاد مطلوب");

        if (user.DateOfBirth != default && user.DateOfBirth > DateTime.Now)
            errors.Add("تاريخ الميلاد غير صحيح");

        if (user.DateOfBirth != default || user.DateOfBirth.Year < 2008)
            errors.Add("تاريخ الميلاد غير صحيح");


        return errors;
    }

    private List<string> ValidateUser(UserToAddDTO user)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(user.UserName))
            errors.Add("اسم المستخدم مطلوب");
        else if (user.UserName.Length < 3)
            errors.Add("اسم المستخدم يجب أن يكون 3 أحرف على الأقل");

        if (string.IsNullOrWhiteSpace(user.Password))
            errors.Add("كلمة المرور مطلوبة");
        else if (user.Password.Length < 3)
            errors.Add("كلمة المرور يجب أن تكون 3 أحرف على الأقل");

        if (user.Password != user.ConfirmPassword)
            errors.Add("كلمة المرور غير متطابقة");

        if (string.IsNullOrWhiteSpace(user.FirstName))
            errors.Add("الاسم الأول مطلوب");

        if (string.IsNullOrWhiteSpace(user.LastName))
            errors.Add("الاسم الأخير مطلوب");

        if (string.IsNullOrWhiteSpace(user.Email))
            errors.Add("البريد الإلكتروني مطلوب");
        else if (!IsValidEmail(user.Email))
            errors.Add("البريد الإلكتروني غير صحيح");

        if (!string.IsNullOrWhiteSpace(user.Phone) && !IsValidPhone(user.Phone))
            errors.Add("رقم الهاتف غير صحيح");

        if (user.DateOfBirth == default)
            errors.Add("تاريخ الميلاد مطلوب");

        if (user.DateOfBirth != default && user.DateOfBirth > DateTime.Now)
            errors.Add("تاريخ الميلاد غير صحيح");

        if (user.DateOfBirth != default || user.DateOfBirth.Year < 2008)
            errors.Add("تاريخ الميلاد غير صحيح");

        

        return errors;
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private bool IsValidPhone(string phone)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\+?\d{7,15}$");
    }

}