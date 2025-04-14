namespace NetCoreAI.Project02_APIConsumeUI.Dtos
{
	public class GetByIdCustomerDto
	{
		public int CustomerId { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public decimal Balance { get; set; }
	}
}
