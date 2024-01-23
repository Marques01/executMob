namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }

        public IRolesRepository RoleRepository { get; }

        public IBreakdownRepository BreakdownRepository { get; }

        public IBreakdownImagesRepository BreakdownImagesRepository { get; }

        public IVehicleRepository VehicleRepository { get; }

        Task<bool> CommitAsync();
    }
}
