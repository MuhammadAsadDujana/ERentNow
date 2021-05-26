using Core.Common.Helper;
using Core.Dto;
using Core.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.ReportingService
{
    public class ReportingService
    {
        eRentEntities _db;
        public ReportingService()
        {
            _db = new eRentEntities();
        }

        public IEnumerable<ReservationHeaderDto> getAllReservationList()
        {
            var data = _db.ReservationHdrs.Select(x => new ReservationHeaderDto
            {
                EmailAddress = x.User.EmailAddress,
                FirstName = x.User.FirstName,
                DocumentNumber = x.DocumentNumber,
                CustomerPO = x.CustomerPO,
                TotalRentAmount = x.TotalRentAmount,
                ReservationStatus = x.ReservationStatu.Status,
                ReservationHdrsId = x.id,
                Insurance = x.Insurance
            }).ToList();

            if (data != null)
                return data;
            else
                return null;
        }

        public IEnumerable<ReservationDetailsDto> getReservationDetailById(int ReservationId)
        {
            var data = _db.ReservationDtls.Where(x => x.ReservationID == ReservationId).Select(x => new ReservationDetailsDto
            { Picture = x.ForkliftsModel.Picture ,
              Title =  x.ForkliftsModel.Title,
                Make = x.ForkliftsModel.Make,
                Model =  x.ForkliftsModel.Model,
                Quantity = x.Quantity,
                StartDate = x.StartDate,
                ReturnDate=   x.ReturnDate,
                Location = x.Location,
                RentPrice =  x.RentPrice,
                ReservationStatus = x.ReservationHdr.ReservationStatu.Status,
                ReservationId = x.ReservationHdr.id
            }).ToList();

            if (data != null)
                return data;
            else
                return null;
        }

        public IEnumerable<ReservationStatu> GetReservationStatusList()
        {
            try
            {

                var List = _db.ReservationStatus.ToList();
                if (List != null)
                    return List;
                else
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string UpdateReservationStatusService(int ReservationId, int StatusId)
        {
            var ReservationHdrs = _db.ReservationHdrs.Where(x => x.id == ReservationId).FirstOrDefault();
            if(ReservationHdrs != null)
            {
                ReservationHdrs.ReservationStatusId = StatusId;
                ReservationHdrs.UpdatedOn = DateTime.Now;

                _db.Entry(ReservationHdrs).State = EntityState.Modified;
                _db.SaveChanges();

                return ConstantMessages.Success;
            }
            else
            {
                return ConstantMessages.Failed;
            }
        }
    }
}
