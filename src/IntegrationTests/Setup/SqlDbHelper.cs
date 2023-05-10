using Domain.Entities;
using Domain.Primitives;
using Infrastructure.EntityFramework;
using Infrastructure.EntityFramework.Helpers;
using Infrastructure.EntityFramework.Models;
using Infrastructure.UserRepository;
using Infrastructure.UserRepository.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace IntegrationTests.Setup;

public class SqlDbHelper
{
    public User SampleUser { get; } = new(new UserId("8fa1da92-79d2-48f3-9911-c3f70a9c3e16"),
        new Username("NotIlya"), new Email("sample@email.com"));

    public UserData SampleUserData { get; } = new("8fa1da92-79d2-48f3-9911-c3f70a9c3e16", "NotIlya",
        "sample@email.com", "$2a$12$HodqSbpPxbXBWvW.9RjdsOcY1cWo2eiooHgBniGl443AkpZE4DZv2");

    public Password SampleUserPassword { get; } = new("Password1");
    public string SampleUserPasswordHash { get; } = "$2a$12$HodqSbpPxbXBWvW.9RjdsOcY1cWo2eiooHgBniGl443AkpZE4DZv2";
    
    private readonly AppDbContext _context;
    private readonly IUserRepository _repository;
    private readonly DataMapper _mapper;

    public SqlDbHelper(AppDbContext context)
    {
        _context = context;
        _mapper = new DataMapper();
        _repository = new UserRepository(context, _mapper);
    }

    public async Task Migrate()
    {
        await _context.Database.MigrateAsync();
    }

    public async Task Seed()
    {
        await EnsureUserInDb(SampleUserData);
    }

    public async Task EnsureUserInDb(User user, string password)
    {
        EntityEntry<UserData>? entry = _context.ChangeTracker.Entries<UserData>().FirstOrDefault(e => e.Entity.Id == user.Id.ToString());
        if (entry is not null)
        {
            entry.State = EntityState.Detached;
        }
        
        try
        {
            await _repository.GetById(user.Id);
        }
        catch (UserNotFoundException)
        {
            await _repository.Insert(user, password);
        }
    }

    public async Task EnsureUserInDb(UserData user)
    {
        await EnsureUserInDb(_mapper.MapUser(user), user.PasswordHash);
    }

    public async Task Reload()
    {
        await Drop();
        await Migrate();
        await Seed();
    }
    
    public async Task Drop()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}