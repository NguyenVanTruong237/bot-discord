using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infrastructure.ApplicationDbContext;

namespace Infrastructure
{
    public class Severs
    {
        private readonly ApplicationDbContext _context;
        public Severs(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ModifyWelcomeAsync(ulong id, ulong channelId)
        {
            var sever = await _context.Severs.FindAsync(id);

            if (sever == null)
            {
                _context.Add(new Sever
                {
                    Id = id,
                    Welcome = channelId
                });
            }
            else
            {
                sever.Welcome = channelId;
            }
            await _context.SaveChangesAsync();
        }

        public async Task ClearWelcomeAsync (ulong id)
        {
            var sever = await _context.Severs.FindAsync(id);

            sever.Welcome = 0;
            await _context.SaveChangesAsync();
        }

        public async Task<ulong> GetWelcomeAsync (ulong id)
        {
            var sever = await _context.Severs.FindAsync(id);
            if (sever == null)
            {
                _context.Severs.Add(new Sever
                {
                    Id = id
                });
                await _context.SaveChangesAsync();
            }
            var sever2 = await _context.Severs.FindAsync(id);
            return await Task.FromResult(sever2.Welcome);
        }

        public async Task ModifyBackgroundAsync(ulong id, string url)
        {
            var sever = await _context.Severs.FindAsync(id);

            if (sever == null)
            {
                _context.Add(new Sever
                {
                    Id = id,
                    Background = url
                });
            }
            else
            {
                sever.Background = url;
            }
            await _context.SaveChangesAsync();
        }

        public async Task ClearBackgroundAsync(ulong id)
        {
            var sever = await _context.Severs.FindAsync(id);

            sever.Background = null;
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetBackgroundAsync(ulong id)
        {
            var sever = await _context.Severs.FindAsync(id);

            return await Task.FromResult(sever.Background);
        }
    }
}
