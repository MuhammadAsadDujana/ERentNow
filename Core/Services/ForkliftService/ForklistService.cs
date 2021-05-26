using Core.Common.Helper;
using Core.Dto;
using Core.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.ForkliftService
{
    public class ForklistService : IForklistService
    {
        eRentEntities _db;

        public ForklistService()
        {
            _db = new eRentEntities();
        }

        public IEnumerable<Forklift> GetForkliftsList()
        {
            try
            {

                var List = _db.Forklifts.OrderByDescending(x => x.CreatedBy).ToList();
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

        public Forklift InsertForklift(Forklift entity)
        {
            if (entity == null)
                return null;

            _db.Forklifts.Add(entity);
            _db.SaveChanges();
            //_db.Entry(entity).State = EntityState.Added;
            //_db.SaveChangesAsync();
            return entity;
        }

        public Forklift GetForkliftById(int forkliftId)
        {
            try
            {

                var data = _db.Forklifts.Where(x => x.Id == forkliftId).FirstOrDefault();
                if (data != null)
                    return data;
                else
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Forklift UpdateForklift(Forklift entity)
        {
            if (entity == null)
                return null;

            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChangesAsync();
            return entity;
        }

        public virtual List<Forklift> GetForkliftInfo(Forklift forklift = null)
        {
            try
            {
                var result = _db.Database.SqlQuery<Forklift>("Exec sp_Get_Forklifts @Name, @IsActive",
                    new SqlParameter("Name", forklift.Name == null ? "" : forklift.Name),
                    new SqlParameter("IsActive", forklift.IsActive)).ToList();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public virtual List<ForkliftsModel> GetForkliftModel(ForkliftsModel forkliftmodel = null)
        {
            try
            {
                var result = _db.Database.SqlQuery<ForkliftsModel>("Exec sp_Get_ForkliftsModel @forkliftsId, @make, @model, @title, @IsActive",
                    new SqlParameter("forkliftsId", forkliftmodel.ForkliftsId),
                    new SqlParameter("make", forkliftmodel.Make == null ? "" : forkliftmodel.Make),
                    new SqlParameter("model", forkliftmodel.Model == null ? "" : forkliftmodel.Model),
                    new SqlParameter("title", forkliftmodel.Title == null ? "" : forkliftmodel.Title),
                    new SqlParameter("IsActive", forkliftmodel.IsActive)).ToList();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public virtual List<ReservationDto> GetNotificationListService(int UserId)
        {
            try
            {
                var result = _db.Database.SqlQuery<ReservationDto>("Exec sp_NotificationList @UserId",
                    new SqlParameter("UserId", UserId)).ToList();
                if (result != null)
                    return result;
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        
        public virtual IEnumerable<OrderHistoryDto> ViewOrderHistoryByIdService(int userId)
        {
            try
            {
                var result = _db.Database.SqlQuery<OrderHistoryDto>("Exec sp_OrderHistory @userId",
                    new SqlParameter("userId", userId)).ToList();
                if (result != null)
                    return result;
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public virtual ReservationForkliftsModelDto ViewNotificationByIdService(int ReservationId)
        {
            try
            {
                var result = _db.Database.SqlQuery<ReservationForkliftsModelDto>("Exec sp_ViewNotification @ReservationId",
                    new SqlParameter("ReservationId", ReservationId)).FirstOrDefault();
                if (result != null)
                    return result;
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<ForkliftsModel> GetForkliftModelList()
        {
            try
            {

                var List = _db.ForkliftsModels.OrderByDescending(x => x.CreatedBy).ToList();
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

        public virtual List<Reservation> InsertReservations(ReservationModel reservationModel = null)
        {
            try
            {
                var result = _db.Database.SqlQuery<Reservation>("Exec sp_Insert_Reservation @ReservationLine",
                    new SqlParameter("ReservationLine", reservationModel.ReservationLine)).ToList();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ResponseModel InsertForkliftModel(ForkliftModelViewModel viewModel,string saveFilePath)
        {
            if (viewModel == null)
                return null;

            ResponseModel resp = new ResponseModel();
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {


                    ForkliftsModel model = new ForkliftsModel()
                    {
                        ForkliftsId = viewModel.ForkliftId,
                        Picture = saveFilePath == "" ? null : saveFilePath,
                        Make = viewModel.Make,
                        Capacity = viewModel.Capacity,
                        Model = viewModel.ForkliftModel,
                        MaxMeter = viewModel.MaxMeter,
                        Title = viewModel.Title,
                        Description = viewModel.Description,
                        PurchasePrice = viewModel.InsurancePurchasePrice,
                        Quantity = viewModel.Quantity,
                        PricingStandard_Daily = viewModel.PricingStandardDaily,
                        PricingStandard_Weekly = viewModel.PricingStandardgWeekly,
                        PricingStandard_Montly = viewModel.PricingStandardMonthly,
                        PricingNationalAccount_Daily = viewModel.NationalAccountDaily,
                        PricingNationalAccount_Weekly = viewModel.NationalAccountWeekly,
                        PricingNationalAccount_Montly = viewModel.NationalAccountMonthly,
                        CreatedOn = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false
                    };

                    var forkliftResult =  _db.ForkliftsModels.Add(model);
                    _db.SaveChanges();

                    if(forkliftResult.Id > 0)
                    {
                        ForkliftsModelTransportation transportation = new ForkliftsModelTransportation()
                        {
                            ForkliftsModelId = forkliftResult.Id,
                            PickUpFees = viewModel.TransportationPickUp,
                            DropOff_PickUp_Fees = viewModel.TransportationDropOffPickUp,
                            DropOffOnly = viewModel.TransportationDropOffOnly,
                            PickUpOnly = viewModel.TransportationPickUpOnly,
                            LossDamageWaiver = viewModel.InsuranceLossDamageWaiver
                        };

                        _db.ForkliftsModelTransportations.Add(transportation);
                        _db.SaveChanges();

                        List<ForkliftsModelChargesDto> chargeList = new List<ForkliftsModelChargesDto>();
                        chargeList.Add(new ForkliftsModelChargesDto { ForkliftsModelId = forkliftResult.Id, ChargeName = viewModel.lblEnvironmental, Amount = viewModel.Environmental });
                        chargeList.Add(new ForkliftsModelChargesDto { ForkliftsModelId = forkliftResult.Id, ChargeName = viewModel.lblMiscCharges, Amount = viewModel.MiscCharges });

                        foreach (var item in chargeList)
                        {
                            ForkliftsModelCharge modelCharge = new ForkliftsModelCharge()
                            {
                                ForkliftsModelId = forkliftResult.Id,
                                ChargesName = item.ChargeName,
                                Amount = item.Amount
                            };
                            _db.ForkliftsModelCharges.Add(modelCharge);
                        }
                        _db.SaveChanges();

                    }
                    resp.isSuccess = true;
                    resp.msg = "Forklift model added successfully";
                    trans.Commit();
                }
                catch(Exception e)
                {
                    trans.Rollback();
                    resp.isSuccess = false;
                    resp.isError = true;
                    resp.msg = "Something went wrong";
                }
            }

            return new ResponseModel { isSuccess = resp.isSuccess, isError = resp.isError , msg = resp.msg };
        }

        public Forklift DeleteForkliftById(int Id)
        {
            var Forklift = _db.Forklifts.Where(x => x.Id == Id).FirstOrDefault();
            if (Forklift != null)
            {
                Forklift.IsActive = false;
                Forklift.IsDeleted = true;
                _db.Entry(Forklift).State = EntityState.Modified;
                _db.SaveChanges();
            }

            return Forklift;
        }

        public ForkliftsModel DeleteForkliftModelById(int Id)
        {
            var Forklift = _db.ForkliftsModels.Where(x => x.Id == Id).FirstOrDefault();
            if (Forklift != null)
            {
                Forklift.IsActive = false;
                Forklift.IsDeleted = true;
                _db.Entry(Forklift).State = EntityState.Modified;
                _db.SaveChanges();
            }

            return Forklift;
        }

        public ForkliftsModel GetForkliftModelById(int forkliftModelId)
        {
            try
            {

                var data = _db.ForkliftsModels.Where(x => x.Id == forkliftModelId).FirstOrDefault();
                if (data != null)
                    return data;
                else
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ForkliftsModel UpdateForkliftModel(ForkliftsModel entity)
        {
            if (entity == null)
                return null;

            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChangesAsync();
            return entity;
        }

    }
}
