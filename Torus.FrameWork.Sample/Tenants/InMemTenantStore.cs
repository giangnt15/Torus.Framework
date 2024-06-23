using Torus.Framework.Core.MultiTenancy;

namespace Torus.FrameWork.Sample.Tenants
{
    public class InMemTenantStore : ITenantStore
    {
        private readonly List<TenantConfig> Tenants = [new TenantConfig() { Id = Guid.Parse("75c8ff0f-3936-4439-85df-297266f866c5"), Name = "123" }];

        public Task<TenantConfig> GetAsync(string tenantName)
        {
            throw new NotImplementedException();
        }

        public Task<TenantConfig> GetAsync(Guid tenantId)
        {
            return Task.FromResult(Tenants.FirstOrDefault(x=>x.Id == tenantId)!);
        }
    }
}
