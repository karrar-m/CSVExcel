using CSVExcel.Exceptions;
using CSVExcel.Model.Employee;
using FluentValidation;
using Swashbuckle.AspNetCore.Swagger;
using System.Text.RegularExpressions;

namespace CSVExcel.Service;

public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(emp => emp.FirstName)
        .NotEmpty().WithMessage("الاسم الأول مطلوب.")
            .Must(BeAvalidName).WithMessage("الاسم الأول لا يقبل أرقام أو رموز.");

        RuleFor(emp => emp.LastName)
            .NotEmpty().WithMessage("الاسم الثاني مطلوب.")
            .Must(BeAvalidName).WithMessage("الاسم الثاني لا يقبل أرقام أو رموز.");

        RuleFor(emp => emp.ThirdName)
           .NotEmpty().WithMessage("الاسم الثالث مطلوب.")
           .Must(BeAvalidName).WithMessage("الاسم الثالث لا يقبل أرقام أو رموز.");

        RuleFor(emp => emp.Gender)
         .NotEmpty().WithMessage("الجنس مطلوب")
         .Must(g => g == "ذكر" || g == "أنثى").WithMessage("الجنس يجب أن يكون ذكر أو أنثى.");

        RuleFor(emp => emp.Nationality)
                   .NotEmpty().WithMessage("الجنسية مطلوب")
                   .Must(n => n == "عراقي" || n == "اجنبي" || n == "عربي")
                   .WithMessage("الجنسية يجب أن تكون عربي أو أجنبي أو عراقي.");

        RuleFor(emp => emp.Education)
            .Must(e =>
                e == "أمي" ||
                e == "يقرأ ويكتب" ||
                e == "ابتدائي" ||
                e == "متوسط" ||
                e == "ثانوي" ||
                e == "دبلوم" ||
                e == "بكالوريوس" ||
                e == "ماجستير" ||
                e == "دكتوراه").WithMessage("مستوى التعليم غير صالح.");

        RuleFor(emp => emp.HiringType)
     .Must(et =>
         et == "دائمي" ||
        et == "مؤقت" ||
        et == "استشاري" ||
        et == "(حكومي (مجاز 5 سنين"
     )
     .WithMessage("نوع التعيين غير صالح.");


        RuleFor(emp => emp.MonthlySalary)
            .Must(BeAvalidSalary)
            .WithMessage("الراتب يجب أن يكون لا يقل عن 266 دولار أو 350000 عراقي.");


        RuleFor(user => user.Allowances)
            .GreaterThanOrEqualTo(0).WithMessage("الزيادة يجب أن تكون على الأقل صفر.");

        RuleFor(e => e.EmployeeNumber)
         .NotEmpty().WithMessage("رقم الموظف مطلوب.")
         .Length(1, 15).WithMessage("رقم الموظف يجب أن لا يتجاوز 15 مرتبة.")
         .Matches("^[a-zA-Z0-9]+$").WithMessage("رقم الموظف يمكن أن يحتوي فقط على أحرف وأرقام.");


    }


    private bool BeAvalidName(string name)
    {
        return Regex.IsMatch(name, @"^[\p{L} ]+$");
    }

    private bool BeAvalidSalary(decimal salary)
    {
        return salary >= 266 || salary >= 350000;
    }
    public async Task ValidateEmployeesAsync(List<Employee> employees , CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();   
        var errors = new List<string>();

        for ( int row = 0; row < employees.Count; row++)
        {
            var employee = employees[row];  
            var validationResult = await ValidateAsync(employee);
            errors.AddRange(validationResult.Errors.Select(e => e.ErrorMessage));

                foreach (var error in validationResult.Errors) {

                    errors.Add($"خطأ في الصف {row+ 1}: {error.ErrorMessage}"); 

                }
        }

        if (errors.Any())
        {
            throw new ExcelException("التحقق من صحة بيانات الموظف فشل.", errors);
        }
    }
}




