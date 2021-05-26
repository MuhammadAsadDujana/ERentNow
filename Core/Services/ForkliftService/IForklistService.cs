using Core.Dto;
using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.ForkliftService
{
    public interface IForklistService
    {
        IEnumerable<Forklift> GetForkliftsList();
        Forklift InsertForklift(Forklift forklift);
        Forklift GetForkliftById(int forkliftId);
        Forklift UpdateForklift(Forklift entity);
        List<Forklift> GetForkliftInfo(Forklift forklift = null);
        List<ForkliftsModel> GetForkliftModel(ForkliftsModel forkliftmodel = null);
        List<ReservationDto> GetNotificationListService(int UserId);
        ReservationForkliftsModelDto ViewNotificationByIdService(int ReservationId);
        IEnumerable<OrderHistoryDto> ViewOrderHistoryByIdService(int userId);
        IEnumerable<ForkliftsModel> GetForkliftModelList();
    }
}
