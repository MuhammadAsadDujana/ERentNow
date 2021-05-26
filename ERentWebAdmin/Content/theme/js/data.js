//    Main Namespace = SA
var dataAccess = {};

////    This object is responsible to provide constants throughout the project
////    SA Constants
dataAccess.constants = {

    login: "/User/Login",
    GetLoginHistory: "/User/GetLoginHistory",
    logout: "/User/Logout",
    createCompanyProfile: "/User/CreateCompanyProfile",
    deleteCompany: "/User/DeleteCompany",
    EditCompany: "/User/editCompany",
    createForklift: "/Forklift/CreateForklift",
    changeForkliftStatus: "/Forklift/ChangeForkliftStatus",
    sendNotifications: "/Notification/PushNotifcation",
    addForkliftModel: "/Forklift/CreateForkliftModel",
    forgotPassword: "/User/ForgotPassword",
    UpdatePassword: "/User/UpdatePassword",
    changePassword: "/User/ChangePassword",
    addNewUser: "/User/CreateUser",
    deleteUser: "/User/DeleteUser",
    deleteForklift: "/Forklift/deleteForklift",
    deleteForkliftModel: "/Forklift/deleteForkliftModel",
    changeForkliftModelStatus: "/Forklift/ChangeForkliftModelStatus",
    updateReservationStatus: "Reporting/UpdateReservationStatus"
};

////    This object is responsible to provide all authentication related stuff
////    Login Service

dataAccess.loginService = {
   
    login: function () {

        debugger;
        //alert(1);


        $("#LoginErrorMessage").text("");
        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }
        
        $(".loading").fadeIn("slow");

            var remember = $('#ckRemember').prop('checked') ? true : false;
             //   debugger;
                //alert(2);
                $.ajax({
                    type: 'post',
                    data: { Email: $("#email").val(), Password: $("#password").val(), RememberMe: remember },
                    url: dataAccess.constants.login,
                    success: function (res) {
                        //alert(res);
                        debugger;
                      //  var Result = JSON.parse(res);

                        if (res === true) {

                          
                            //   $(".loading").fadeOut("slow");
                            setTimeout(function () {
                                $(".loading").fadeOut("slow");
                            }, 8000);
                            window.location.href = "/User/UserManagement";
                        }
                        else
                        {
                            setTimeout(function () {
                                $(".loading").fadeOut("slow");
                            }, 3000);
                            $("#LoginErrorMessage").text(res);
                            return;
                        }
                    }
                });
    
    },

    forgotPassword: function () {
        //$('#preloader').fadeIn("slow");
        //NProgress.start();

        debugger;
        //alert(1);
        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }

        $.ajax({
            type: 'post',
            data: { email: $("#emailForForgotPassword").val() },
            url: dataAccess.constants.forgotPassword,
            success: function (res) {
               // alert(res);
                debugger;
              //  NProgress.done();
                var Result = JSON.parse(res);

                if (Result.Code === "400") {

                    swal("Server side alert 400", Result.Message, "warning");
                    return;

                } else if (Result.Code === "404") {

                    swal("Server side alert 404", Result.Message, "warning");
                    return;
                }

                else {
                    //  alert("Record deleted");
                    swal({ title: "Email Sent", text: Result.Message, type: "success", allowEscapeKey: false, allowOutsideClick: false },
                        function () {
                            $("#emailForForgotPassword").val("");
                        }
                    );
                }

            }
        });

    },

    UpdatePasswordForm: function () {

        debugger;
        //alert(1);
        var $form = $('#UpdatePasswordForm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }


        if ($('[name= Password]').val() != $('[name= ConfirmPassword]').val()) {
            swal("Alert", "Password & confirm password must be matched", "warning");
        } else {
            $.post(dataAccess.constants.UpdatePassword, $('#UpdatePasswordForm').serialize()).done(function (resp) {
                if (resp.isSuccess) {
                   // window.location.href = '/User/Login';
                        swal({ title: "Done", text: "You password has been updated!", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                            function () {
                                window.location.href = '/User/Login';
                                //  NProgress.done();
                            }
                        );
                }
                else {
                    swal("Error", resp.msg, "warning");
                }

            }).fail(function (data) {
                console.log(data)
            });
        }

    },

    logout: function () {
        debugger;
        swal({
            title: "Alert",
            text: "Are you sure you want to logout",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Logout",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {

                    $.ajax({
                        type: 'get',
                        url: dataAccess.constants.logout,
                        async: false,
                        success: function (res) {
                            debugger;
                            //       var Result = JSON.parse(res);
                            if (res === true) {
                                swal({ title: "Done", text: "You are logged out!", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/User/Login';
                                        //  NProgress.done();
                                    }
                                );
                            }

                        }
                    });


                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });  

    }

 
};

dataAccess.userService = {

    UserLoginHistory: function () {
        debugger;
        //alert(1);
     
        $.ajax({
            type: "GET",
            datatype: "json",
            contentType: "application/json;charset=utf-8",
            processData: true,
            url: dataAccess.constants.GetLoginHistory,
            success: function (data) {
                debugger;
             //   var Result = JSON.parse(data);
                var user = '';

                // ITERATING THROUGH OBJECTS 
                $.each(data, function (key, value) {

                    //CONSTRUCTION OF ROWS HAVING 
                    // DATA FROM JSON OBJECT 
                    var dateObject = new Date(parseInt(value.LoginTime.substr(6)));
                    var formattedLoginTime = dateObject.toLocaleDateString() + ' ' + dateObject.toLocaleTimeString();

                    user += '<tr>';
                    user += '<td>' +
                        value.FirstName + ' ' + value.LastName + '</td>';

                    user += '<td>' +
                        value.EmailAddress + '</td>';

                    user += '<td>' +
                        value.JobTitle + '</td>';

                    user += '<td>' +
                        value.Role + '</td>';

                    user += '<td>' +
                        formattedLoginTime + '</td>';

                    //if (value.Offline == true) {
                    //    user += '<td>' +
                    //        'Online' + '</td>';
                    //}
                    //else {
                    //    user += '<td>' +
                    //        'Offline' + '</td>';
                    //}
                   

                    user += '</tr>';
                });

                //INSERTING ROWS INTO TABLE  
                $('#myTable').append(user); 


                $('#table').DataTable({
                    //"scrollY": "68vh",
                    //"scrollCollapse": false,
                    "searching": true,
                    //"paging": true,
                    autoWidth: true,
                    responsive: true
                });

            }
        });
        function array(arr) {
            console.log(arr);
        }

    },

    EditCompany: function () {

        debugger;
        //   alert(1);

        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }
 
        swal({
            title: "Alert",
            text: "Are you sure you want to edit this record?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Edit",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    debugger;
                  
                    var file = $("#imageUpload").get(0).files;

                    var data = new FormData;
                    data.append("Logo", file[0]);
                    data.append("ProfileLogo", $('#txtProfileLogo').val());
                    data.append("CompanyId", $('#txtCompanyId').val());
                    data.append("Header", $("#txtAreaHeader").val());
                    data.append("Footer", $("#txtFooter").val());
                    data.append("BannerMassage", $("#txtBannerMessage").val());
                    data.append("TermsOfUse", $("#txtAreaTermsOfUse").val());
                    data.append("PrivacyPolicy", $("#txtAreaPrivacypolicy").val());


                    $.ajax({
                        url: dataAccess.constants.EditCompany,
                        async: false,
                        type: 'POST',
                        data: data,
                        //   contentType: "application/json; charset=utf-8",
                        processData: false,  // tell jQuery not to process the data
                        contentType: false,
                        //       datatype: "json",
                        success: function (res) {
                           
                            debugger;

                            if (res === true) {
                                swal({ title: "Done", text: "Record updated successfully!", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = "/User/CompanyProfileManagement";
                                    }
                                );
                            }
                            else {
                                swal("Server side error", res, "error");
                                $("#ErrorMessage").text(res);
                                return;
                            }
                        }
                    });


                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });

    },

    CreateCompanyProfile: function () {


        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }
        //  $(".loading").fadeIn("slow");

        debugger;
        //   alert(1);

        swal({
            title: "Alert",
            text: "Are you sure you want to add this record?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Add",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    debugger;
                    var file = $("#imageUpload").get(0).files;

                    var data = new FormData;
                    data.append("Logo", file[0]);
                    data.append("Header", $("#txtAreaHeader").val());
                    data.append("Footer", $("#txtFooter").val());
                    data.append("BannerMassage", $("#txtBannerMessage").val());
                    data.append("TermsOfUse", $("#txtAreaTermsOfUse").val());
                    data.append("PrivacyPolicy", $("#txtAreaPrivacypolicy").val());
                  

                    //  data.append("physician", dataa);

                    $.ajax({
                        url: dataAccess.constants.createCompanyProfile,
                        async: false,
                        type: 'POST',
                        data: data,
                        //  contentType: "multipart/form-data",
                        processData: false,  // tell jQuery not to process the data
                        contentType: false,
                        //    datatype: "json",
                        success: function (res) {
                            //var Result = JSON.parse(res);
                            debugger;

                            if (res === true) {
                                swal({ title: "Done", text: "Record added successfully!", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = "/User/CompanyProfileManagement";
                                    }
                                );
                            }
                            else {
                                swal("Server side error", res, "error");
                                $("#ErrorMessage").text(res);
                                return;
                            }

                            //});
                        }
                    });

                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });

    },

    DeleteCompany: function (Id) {
        //  $('#preloader').fadeIn("slow");
      //  alert(1);
        debugger;

        swal({
            title: "Alert",
            text: "Are you sure you want to delete this record?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Delete",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    debugger;
                    var data = {
                        companyId: Id
                    };

                    $.ajax({
                        url: dataAccess.constants.deleteCompany,
                        async: false,
                        type: 'POST',
                        data: JSON.stringify(data),
                        contentType: "application/json; charset=utf-8",
                        datatype: "json",
                        success: function (res) {
                           // var Result = JSON.parse(res);
                            debugger;


                            if (res === true) {
                                swal({ title: "Done", text: "Record is deleted.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/User/CompanyProfileManagement';
                                    }
                                );
                            }
                            else {
                                swal("Server side error", res, "error");
                             //   $("#ErrorMessage").text(res);
                                return;
                            }

                        }
                    });

                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });


    },

    CreateForklift: function () {


        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }
        //  $(".loading").fadeIn("slow");

        debugger;
        //   alert(1);

        swal({
            title: "Alert",
            text: "Are you sure you want to add this record?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Add",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    debugger;
                    var file = $("#imageUpload").get(0).files;

                    var data = new FormData;
                    data.append("PictureWrapper", file[0]);
                    data.append("Name", $("#txtName").val());
                    data.append("Description", $("#txtDescription").val());
               

                    $.ajax({
                        url: dataAccess.constants.createForklift,
                        async: false,
                        type: 'POST',
                        data: data,
                        //  contentType: "multipart/form-data",
                        processData: false,  // tell jQuery not to process the data
                        contentType: false,
                        //    datatype: "json",
                        success: function (res) {
                            //var Result = JSON.parse(res);
                            debugger;

                            if (res === true) {
                                swal({ title: "Done", text: "Record added successfully!", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = "/Forklift/ForkliftManagement";
                                    }
                                );
                            }
                            else {
                                swal("Server side error", res, "error");
                                $("#ErrorMessage").text(res);
                                return;
                            }

                            //});
                        }
                    });

                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });

    },

    changeStatus: function (ForkliftId, Status, previousCheckboxValue) {
        //$('#preloader').fadeIn("slow");
        //NProgress.start();
        debugger;


        swal({
            title: "Alert",
            text: "Are you sure you want to change status?",
            type: "info",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ok",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {

                    debugger;
                    //var data = { UserId: UserId, isChecked: isChecked };

                    $.ajax({
                        type: 'post',
                        data: { forkliftId: ForkliftId, status: Status },
                        url: dataAccess.constants.changeForkliftStatus,
                        async: false,
                        //   traditional: true,
                        success: function (res) {
                            //  NProgress.done();
                            debugger;

                            if (res === true) {
                                swal({ title: "Done", text: "Record updated successfully!", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = "/Forklift/ForkliftManagement";
                                    }
                                );
                            }
                            else {
                                swal("Server side error", res, "error");
                                $("#ErrorMessage").text(res);
                                return;
                            }

                        }
                    });

                }
                else {
                      debugger;
                    if (Status == false)
                        previousCheckboxValue.checked = true;
                    else
                        previousCheckboxValue.checked = false;

                }
            });

    },

    CreateNewUser: function (e) {

        debugger;
        //alert(1);
      //  var dfsd = $('#myfrm').serialize();
        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }

        swal({
            title: "Alert",
            text: "Are you sure you want to add this user?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Send",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {

                    debugger;

                    var formData = new FormData(e);


                    $.ajax({
                        url: dataAccess.constants.addNewUser,
                        type: "POST",
                        dataType: "JSON",
                        data: formData,
                        cache: false,
                        contentType: false,
                        processData: false,
                        success: function (res) {
                            debugger;
                          //  alert(1);
                            if (res.isSuccess) {

                                swal({ title: "Done", text: "User added successfully.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/User/UserManagement';
                                    }
                                );
                            }
                            else {
                                swal("Error", res.msg, "warning");
                                return;
                            }

                        },
                        error: function (response) {
                            alert(response.responseText);
                        }
                    });
                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            },
        );

    },

    DeleteUser: function (Id) {
        //  $('#preloader').fadeIn("slow");
        //  alert(1);
        debugger;

        swal({
            title: "Alert",
            text: "Are you sure you want to delete this record?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Delete",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    debugger;
                    var data = {
                        userId: Id
                    };

                    $.ajax({
                        url: dataAccess.constants.deleteUser,
                        async: false,
                        type: 'POST',
                        data: JSON.stringify(data),
                        contentType: "application/json; charset=utf-8",
                        datatype: "json",
                        success: function (res) {
                            debugger;
                            if (res === true) {
                                swal({ title: "Done", text: "Record is deleted.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/User/UserManagement';
                                    }
                                );
                            }
                            else {
                                swal("Server side error", res, "error");
                                //   $("#ErrorMessage").text(res);
                                return;
                            }

                        }
                    });

                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });

    }

};

dataAccess.changePasswordService = {

    updatePassword: function () {

        debugger;

        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }

        var data = {
            OldPassword: $("#CurrentPassword").val(),
            NewPassword: $("#NewPassword").val(),
            ConfirmPassword: $("#ConfirmPassword").val()
        };

        var getdata = { model: data };

        $.ajax({
            type: 'post',
            data: getdata,
            url: dataAccess.constants.changePassword,
            success: function (resp) {
                //  var Result = JSON.parse(resp);
                debugger;
               // alert(1);
                if (resp.isSuccess) {
      
                    $("#CurrentPassword").val("");
                    $("#NewPassword").val("");
                    $("#ConfirmPassword").val("");
                    swal("Done", resp.msg, "success");
                }
                else {
                    swal("Error", resp.msg, "warning");
                }

            },
            error: function () {
                alert("error");
            }
        });
    }
};

dataAccess.forkliftService = {

    addforkliftModel: function (e) {

        debugger;
        //alert(1);

        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }

        swal({
            title: "Alert",
            text: "Are you sure you want to send notification?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Send",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                  
                    debugger;

                    var formData = new FormData(e);


                    $.ajax({
                        url: dataAccess.constants.addForkliftModel,
                        type: "POST",
                        dataType: "JSON",
                        data: formData,
                        cache: false,
                        contentType: false,
                        processData: false,
                        success: function (res) {
                            debugger;
                            alert(1);
                            if (res.isSuccess) {

                                swal({ title: "Done", text: "Forklift added successfully.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/Forklift/ForkliftModelManagement';
                                    }
                                );
                            }
                            else {
                                swal("Error", res.msg, "warning");
                                return;
                            }

                        },
                        error: function (response) {
                            alert(response.responseText);
                        }
                    });
                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            },
        );


    },

    DeleteForklift: function (Id) {
        //  $('#preloader').fadeIn("slow");
        //  alert(1);
        debugger;

        swal({
            title: "Alert",
            text: "Are you sure you want to delete this record?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Delete",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    debugger;
                    var data = {
                        forkliftId: Id
                    };

                    $.ajax({
                        url: dataAccess.constants.deleteForklift,
                        async: false,
                        type: 'POST',
                        data: JSON.stringify(data),
                        contentType: "application/json; charset=utf-8",
                        datatype: "json",
                        success: function (res) {
                            debugger;
                            if (res === true) {
                                swal({ title: "Done", text: "Record is deleted.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/Forklift/ForkliftManagement';
                                    }
                                );
                            }
                            else {
                                swal("Server side error", res, "error");
                                //   $("#ErrorMessage").text(res);
                                return;
                            }

                        }
                    });

                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });

    },

    DeleteForkliftModel: function (Id) {
        //  $('#preloader').fadeIn("slow");
        //  alert(1);
        debugger;

        swal({
            title: "Alert",
            text: "Are you sure you want to delete this record?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Delete",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    debugger;
                    var data = {
                        forkliftModelId: Id
                    };

                    $.ajax({
                        url: dataAccess.constants.deleteForkliftModel,
                        async: false,
                        type: 'POST',
                        data: JSON.stringify(data),
                        contentType: "application/json; charset=utf-8",
                        datatype: "json",
                        success: function (res) {
                            debugger;
                            if (res === true) {
                                swal({ title: "Done", text: "Record is deleted.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = '/Forklift/ForkliftModelManagement';
                                    }
                                );
                            }
                            else {
                                swal("Server side error", res, "error");
                                //   $("#ErrorMessage").text(res);
                                return;
                            }

                        }
                    });

                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            });

    },

    ChangeForkLiftModelStatus: function (ForkliftId, Status, previousCheckboxValue) {
        //$('#preloader').fadeIn("slow");
        //NProgress.start();
        debugger;


        swal({
            title: "Alert",
            text: "Are you sure you want to change status?",
            type: "info",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Ok",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {

                    debugger;
                    //var data = { UserId: UserId, isChecked: isChecked };

                    $.ajax({
                        type: 'post',
                        data: { forkliftId: ForkliftId, status: Status },
                        url: dataAccess.constants.changeForkliftModelStatus,
                        async: false,
                        //   traditional: true,
                        success: function (res) {
                            //  NProgress.done();
                            debugger;

                            if (res === true) {
                                swal({ title: "Done", text: "Record updated successfully!", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        window.location.href = "/Forklift/ForkliftManagement";
                                    }
                                );
                            }
                            else {
                                swal("Server side error", res, "error");
                                $("#ErrorMessage").text(res);
                                return;
                            }

                        }
                    });

                }
                else {
                    debugger;
                    if (Status == false)
                        previousCheckboxValue.checked = true;
                    else
                        previousCheckboxValue.checked = false;

                }
            });

    }

};

dataAccess.Notifications = {

    sendNotification: function () {

        debugger;
        //alert(1);

        var $form = $('#myfrm');
        if ($form.parsley().validate() == false) {
            stopPropagation();
            return false;
        }

        swal({
            title: "Alert",
            text: "Are you sure you want to send notification?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Send",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {


                    $.ajax({
                        url: dataAccess.constants.sendNotifications,
                        async: false,
                        type: 'POST',
                        //   data: JSON.stringify(model),
                        data: { title: $("#txtTitle").val(), message: $("#txtMessage").val() },
                        //    contentType: "application/json; charset=utf-8",
                        //   datatype: "json",
                        success: function (res) {
                            debugger;

                            //if (res == true)
                            //    alert(1);

                            if (res == true) {
                                swal({ title: "Done", text: "Notification is sent successfully.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        $("#txtTitle").val("");
                                        $("#txtMessage").val("");
                                        $("#ErrorMessage").val("");                                        
                                        window.location.href = '/Notification/PushNotifcation';
                                    }
                                );
                            }
                            else {
                                swal("Server side error", "Something went wrong", "error");
                                $("#ErrorMessage").text("Error !");
                                return;
                            }
                           

                        },
                        error: function (response) {
                            alert(response.responseText);
                        }
                    });
                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            },
        );


    }

};

dataAccess.reportingService = {

    UpdateReservationStatus: function () {

        debugger;
        //alert(1);

        swal({
            title: "Alert",
            text: "Are you sure you want to update?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Send",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {


                    $.ajax({
                        url: dataAccess.constants.updateReservationStatus,
                        async: false,
                        type: 'POST',
                        //   data: JSON.stringify(model),
                        data: { ReservationId: $("#ReservationId").val(), StatusId: $("#StatusId").val() },
                        //    contentType: "application/json; charset=utf-8",
                        //   datatype: "json",
                        success: function (res) {
                            debugger;

                            //if (res == true)
                            //    alert(1);

                            if (res == true) {
                                swal({ title: "Done", text: "Reservation has been updated.", type: "success", allowEscapeKey: false, allowOutsideClick: false },
                                    function () {
                                        $("#txtTitle").val("");
                                        $("#txtMessage").val("");
                                        $("#ErrorMessage").val("");
                                        window.location.href = '/Reporting/BillingHistroy';
                                    }
                                );
                            }
                            else {
                                swal("Server side error", "Something went wrong", "error");
                                $("#ErrorMessage").text("Error !");
                                return;
                            }


                        },
                        error: function (response) {
                            alert(response.responseText);
                        }
                    });
                }
                //else {
                //    swal("Cancelled", "You have Cancelled Form Submission!", "error");
                //}
            },
        );


    }

};


function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}
