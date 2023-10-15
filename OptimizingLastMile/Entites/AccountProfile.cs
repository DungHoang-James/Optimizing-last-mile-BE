using System;
using OptimizingLastMile.Models.Commons;

namespace OptimizingLastMile.Entites;

public class AccountProfile
{
    public long Id { get; private set; }
    public string Name { get; private set; }
    public DateTime? BirthDay { get; private set; }
    public string Province { get; private set; }
    public string District { get; private set; }
    public string Ward { get; private set; }
    public string Address { get; private set; }
    public string PhoneContact { get; private set; }

    protected AccountProfile() { }

    private AccountProfile(string name,
        DateTime? birthDay,
        string province,
        string district,
        string ward,
        string address,
        string phoneContact)
    {
        Name = name.Trim();
        BirthDay = birthDay;
        Province = province.Trim();
        District = district.Trim();
        Ward = ward.Trim();
        Address = address.Trim();
        PhoneContact = phoneContact.Trim();
    }

    public static GenericResult<AccountProfile> Create(string name,
        DateOnly? birthDay,
        string province,
        string district,
        string ward,
        string address,
        string phoneContact)
    {
        DateTime? dob = null;

        if (birthDay.HasValue)
        {
            dob = birthDay.Value.ToDateTime(TimeOnly.MinValue);

            var validateBirthDay = IsBirthDayValid(dob.Value);
            if (validateBirthDay.IsFail)
            {
                return GenericResult<AccountProfile>.Fail(validateBirthDay.Error);
            }
        }

        return GenericResult<AccountProfile>.Ok(new(name, dob, province, district, ward, address, phoneContact));
    }

    private static GenericResult IsBirthDayValid(DateTime birthDay)
    {
        if (birthDay >= DateTime.UtcNow)
        {
            var error = Errors.AccountProfile.InvalidBirthDay();
            return GenericResult.Fail(error);
        }

        return GenericResult.Ok();
    }

    public void SetName(string name)
    {
        if (name is not null)
        {
            Name = name.Trim();
        }
    }

    public GenericResult SetBirthDay(DateOnly? birthDay)
    {
        if (!birthDay.HasValue)
        {
            BirthDay = null;
            return GenericResult.Ok();
        }

        var dob = birthDay.Value.ToDateTime(TimeOnly.MinValue);

        var validateBirthDay = IsBirthDayValid(dob);
        if (validateBirthDay.IsFail)
        {
            return GenericResult.Fail(validateBirthDay.Error);
        }

        BirthDay = dob;

        return GenericResult.Ok();

    }

    public void SetProvince(string province)
    {
        if (province is not null)
        {
            Province = province.Trim();
        }
        else
        {
            Province = province;
        }
    }

    public void SetDistrict(string district)
    {
        if (district is not null)
        {
            District = district.Trim();
        }
        else
        {
            District = district;
        }
    }

    public void SetWard(string ward)
    {
        if (ward is not null)
        {
            Ward = ward.Trim();
        }
        else
        {
            Ward = ward;
        }
    }

    public void SetAddress(string address)
    {
        if (address is not null)
        {
            Address = address.Trim();
        }
        else
        {
            Address = address;
        }
    }

    public void SetPhoneContact(string phoneContact)
    {
        if (phoneContact is not null)
        {
            PhoneContact = phoneContact;
        }
        else
        {
            PhoneContact = phoneContact;
        }
    }
}

