using FluentValidation.TestHelper;
using Xunit;

public class CreateContactValidatorTests
{
    [Fact]
    public void Invalid_email_should_fail()
    {
        var v = new CreateContactValidator();
        var r = v.TestValidate(new CreateContactCommand("Ana", "x", "+55 81999999999"));
        r.ShouldHaveValidationErrorFor(x => x.Email);
    }
}
