﻿namespace comp1640_dotnet.Models
{
	public class Role
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string Name { get; set; } = string.Empty;
	}
}