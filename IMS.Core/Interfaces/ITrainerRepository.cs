using IMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Core.Interfaces;
public interface ITrainerRepository
{
    Task AddAsync(Trainer trainer);
}