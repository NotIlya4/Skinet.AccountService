using Domain.Entities;
using Domain.Primitives;
using Infrastructure.EntityFramework;
using Infrastructure.UserRepository;
using IntegrationTests.Setup;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Infrastructure;

[Collection(nameof(AppFixture))]
public class UserRepositoryTests : IDisposable
{
    private readonly IServiceScope _scope;
    private readonly IUserRepository _repository;
    private readonly SqlDbHelper _sqlDbHelper;
    private readonly User _user = new(new UserId("eae5c7c3-77bf-41b4-8f68-d5dd38707bc3"), new Username("Biba"), new Email("biba@email.com"));
    private readonly string _passwordHash = "$2a$12$Wmlz/HtTiR5uVT48eedTyeBna6f8DC1H4zHjXSvKygM0NoWDJY5BO";

    public UserRepositoryTests(AppFixture fixture)
    {
        _scope = fixture.CreateScope();
        _repository = _scope.ServiceProvider.GetRequiredService<IUserRepository>();
        _sqlDbHelper = new SqlDbHelper(_scope.ServiceProvider.GetRequiredService<AppDbContext>());
    }

    [Fact]
    public async Task Get_ById_ReturnSampleUser()
    {
        User result = await _repository.GetById(_sqlDbHelper.SampleUser.Id);
        
        Assert.Equal(_sqlDbHelper.SampleUser, result);
    }
    
    [Fact]
    public async Task Get_ByUsername_ReturnSampleUser()
    {
        User result = await _repository.GetByUsername(_sqlDbHelper.SampleUser.Username);
        
        Assert.Equal(_sqlDbHelper.SampleUser, result);
    }
    
    [Fact]
    public async Task Get_ByEmail_ReturnSampleUser()
    {
        User result = await _repository.GetByEmail(_sqlDbHelper.SampleUser.Email);
        
        Assert.Equal(_sqlDbHelper.SampleUser, result);
    }

    [Fact]
    public async Task Insert_AddUser_GetReturnsThatUser()
    {
        await _repository.Insert(_user, _passwordHash);

        User result = await _repository.GetById(_user.Id);
        string resultPassword = await _repository.GetPasswordHash(_user.Id);
        
        Assert.Equal(_user, result);
        Assert.Equal(_passwordHash, resultPassword);

        await _sqlDbHelper.Reload();
    }

    [Fact]
    public async Task Insert_ThatUserAlreadyExist_OverridePreviousUser()
    {
        User expect = new User(_sqlDbHelper.SampleUser.Id, _user.Username, _user.Email);
        await _repository.Insert(expect, _passwordHash);

        User result = await _repository.GetById(_sqlDbHelper.SampleUser.Id);
        string resultPassword = await _repository.GetPasswordHash(_sqlDbHelper.SampleUser.Id);
        
        Assert.Equal(expect, result);
        Assert.Equal(resultPassword, _passwordHash);
        
        await _sqlDbHelper.Reload();
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}