import React, { useState } from "react";
import share from "../../../assets/sharenlogin.svg";
import instractor from "../../../assets/instractorregister.svg";
import student from "../../../assets/studentinregister.svg";
import { Link } from "react-router-dom";
import loginbg from "../../../assets/loginbg.svg";


const Register = () => {
  const [phone, setPhone] = useState("");
  const [selectedOption, setSelectedOption] = useState("");
  const [showModal, setShowModal] = useState(false);

  const openModal = () => {
    setShowModal(true);
  };
  const handleChangemember = (e) => {
    setSelectedOption(e.target.value);
    setShowModal(true);
  };

  const handleChangeselect = (e) => {
    setSelectedOption(e.target.value);
  };
  const handleChange = (e) => {
    const formattedPhone = e.target.value.replace(/[^0-9]/g, "");
    setPhone(formattedPhone);
  };
  return (
    <>
      <Link
        className=" d-flex align-items-center gap-1"
        to={"/"}
        style={{
          fontSize: "20px",
          fontWeight: "bold",
          color: "#D9D9D9",
          position: "absolute",
          top: "3%",
          left: "21%",
        }}
      >
        <img src={share} alt="المحتويات" style={{ height: "10px" }} />
        الصفحة الرئيسية{" "}
      </Link>
      <div
        className="container-fluid d-flex justify-content-center align-items-center min-vh-100"
        style={{   width:"auto" }}
      >
      
         <div
                 className="row  shadow-lg  overflow-hidden "
                 style={{ width: "65%" , backgroundColor: "#102D4D"  }}
               >
                 <div className="col-md-6 d-flex flex-column justify-content-center align-items-center text-white position-relative">
           <img
             src={loginbg}
             alt="Background Image"
             className="position-absolute top-0 start-0   object-cover"
             style={{ zIndex: 1 ,  height:"85vh", width:"68vh" }}
           />
         
           <div
             className="position-absolute top-0 start-0 "
             style={{ backgroundColor: "#212D5D", opacity: 0.5, zIndex: 2,height:"85vh", width:"68vh" }}
           ></div>
         
           <div className="position-relative z-3" style={{direction:"rtl" , bottom:"33%", left:"15%"}}>
             <h2>"خطوة أقرب إلى حلمك"
             
             </h2>
             <p>"خدمة تعليم إلكتروني مجانية جاهزة
             <br/>
              لمساعدتك على أن تصبح خبيرًا."</p>
           </div>
         </div>

          <div className="col-md-6 p-5  text-white">
            <h4 className="fw-bold text-end">تسجيل مستخدم جديد </h4>
            <p className="text-end">
              "أنشئ حسابك الآن وانطلق في رحلة التعلم والتطوير!"
            </p>
            <form className="mt-4">
              <div className="mb-3">
                <input
                  type="email"
                  className="form-control  bg-transparent text-white border-secondary"
                  placeholder="اسم المستخدم"
                  style={{ direction: "rtl" }}
                />
              </div>
              <div className="mb-3">
                <input
                  type="email"
                  className="form-control  bg-transparent text-white border-secondary"
                  placeholder="الايميل"
                  style={{ direction: "rtl" }}
                />
              </div>
              <div className="mb-3">
                <input
                  type="password"
                  className="form-control  bg-transparent text-white border-secondary"
                  placeholder="كلمة المرور"
                  style={{ direction: "rtl" }}
                />
              </div>
              <div className="mb-3">
                <input
                  type="tel"
                  id="phone"
                  name="phone"
                  value={phone}
                  onChange={handleChange}
                  className="form-control  bg-transparent text-white border-secondary"
                  placeholder="رقم الهاتف"
                  style={{ direction: "rtl" }}
                />
              </div>
              <div className="mb-3 d-flex justify-content-between gap-2">
                <select
                  id="options"
                  value={selectedOption}
                  onChange={handleChangeselect}
                  className="custom-select form-control bg-transparent text-white border-secondary"
                  style={{ direction: "rtl", backgroundColor: "#07203A" }}
                >
                  <option value="" disabled>
                    الجنس{" "}
                  </option>
                  <option value="option1">ذكر</option>
                  <option value="option2">انثي</option>
                  <option value="option3">اخر</option>
                </select>

                <select
                  id="options"
                  value={selectedOption}
                  className="form-control bg-transparent text-white border-secondary"
                  onFocus={openModal}
                  style={{ direction: "rtl" }}
                >
                  <option value="" disabled>
                    النوع
                  </option>
                </select>
              </div>
              <div class="mb-3 form-check  d-flex justify-content-end ">
                <label
                  class="form-check-label"
                  style={{
                    color: "#FCD980",
                    direction: "ltr",
                    marginRight: "9%",
                  }}
                  for="exampleCheck1"
                >
                  الموافقة علي الشروط والاحكام
                </label>
                <input
                  type="checkbox"
                  class="form-check-input bg-transparent"
                  id="exampleCheck1"
                />
              </div>
              <button
                type="submit"
                className="btn w-100 text-white fw-bold"
                style={{ backgroundColor: "#FCD980" }}
              >
                انشاء حساب
              </button>
              <p className="mt-3 text-center text-light">
                لدي حساب مسجل من قبل؟ <a href="#"> تسجيل الدخول</a>
              </p>
            </form>
          </div>
        </div>
        {showModal && (
          <div
            className="modal fade show d-block"
            tabIndex="-1"
            style={{
              backgroundColor: "rgba(0,0,0,0.5)",
              position: "fixed",
              top: 0,
              left: 0,
              width: "100vw",
              height: "100vh",
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              zIndex: 1050,
            }}
          >
            <div className="modal-dialog modal-dialog-centered">
              <div
                className="modal-content text-center p-4  text-white"
                style={{ backgroundColor: "#102D4C" }}
              >
                <h4 className="fw-bold">اختر إذا كنت طالبًا أو محاضرًا</h4>
                <div className="d-flex justify-content-around mt-4">
                  <div
                    className="text-center"
                    onClick={() => {
                      setSelectedOption("lecturer");
                      setShowModal(false);
                    }}
                  >
                    <img
                      src={instractor}
                      alt="محاضر"
                      style={{ cursor: "pointer" }}
                    />
                    <p>محاضر</p>
                  </div>
                  <div
                    className="text-center"
                    onClick={() => {
                      setSelectedOption("student");
                      setShowModal(false);
                    }}
                    style={{ cursor: "pointer" }}
                  >
                    <img
                      src={student}
                      alt="طالب"
                      style={{ cursor: "pointer" }}
                    />
                    <p>طالب</p>
                  </div>
                </div>
                <div className="mt-3 text-center">
                  <button
                    className="btn text-white  mx-2"
                    style={{ backgroundColor: "#07203A" }}
                    onClick={() => setShowModal(false)}
                  >
                    إغلاق
                  </button>
                  <button
                    className="btn  mx-2"
                    style={{ backgroundColor: "#FCD980" }}
                    onClick={() => setShowModal(false)}
                  >
                    تم
                  </button>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    </>
  );
};

export default Register;
