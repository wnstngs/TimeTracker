﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Models;

namespace TimeTracker.Data;

public static class DbContextSeed
{
	public static async Task InitializeAsync(IServiceProvider services)
	{
		var roleManager = services
			.GetRequiredService<RoleManager<ApplicationRole>>();

		await EnsureRolesAsync(roleManager);

		var userManager = services
			.GetRequiredService<UserManager<ApplicationUser>>();

		await EnsureTestAdminAsync(userManager);
	}

	private static async Task EnsureRolesAsync(RoleManager<ApplicationRole> roleManager)
	{
		var adminRoleAlreadyExists = await roleManager
			.RoleExistsAsync(Roles.Admin);

		var userRoleAlreadyExists = await roleManager
			.RoleExistsAsync(Roles.User);


		if (adminRoleAlreadyExists && userRoleAlreadyExists)
		{
			return;
		}

		await roleManager.CreateAsync(new ApplicationRole(Roles.User));
		await roleManager.CreateAsync(new ApplicationRole(Roles.Admin));
	}

	private static async Task EnsureTestAdminAsync(UserManager<ApplicationUser> userManager)
	{
		var testAdmin = await userManager.Users
			.Where(x => x.UserName == TestIdentity.AdminUserName)
			.SingleOrDefaultAsync();
		if (testAdmin != null)
		{
			return;
		}
		testAdmin = new ApplicationUser
		{
			UserName = TestIdentity.AdminUserName,
			Email = TestIdentity.AdminEmail
		};
		await userManager.CreateAsync(
			testAdmin, 
			TestIdentity.AdminPassword);
		await userManager.AddToRoleAsync(
			testAdmin,
			Roles.Admin);
	}
}