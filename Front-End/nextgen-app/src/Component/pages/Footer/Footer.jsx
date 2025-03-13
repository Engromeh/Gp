import React from "react";
import "bootstrap-icons/font/bootstrap-icons.min.css";
import logo from "../../../assets/navlogo.svg";
import langicone from "../../../assets/langua icon.svg"

const Footer = () => {
  return (
    <>
      <footer
        className=" text-light pt-4 "
        style={{ backgroundColor: "#102D4C" }}
      >
        <div className="container">
          <div className="row pb-4">
            <div className="col-md-3 d-flex flex-column align-items-center">
              <img
                src={logo}
                alt="NextGen Academy"
                className="mb-3"
                style={{ width: "200px", marginRight: "320px" }}
              />
               <div className="d-flex justify-content-center gap-4" style={{marginRight:"80%"}}>
 <button
                type="submit"
                className="button-nav btn w-100 w-lg-auto"
                style={{
                  color: "#FFFFFF",
                  border: "2px #D9D9D9 solid",
                 
                  padding: "5px 12px",
                  fontSize: "15px",
                  whiteSpace: "nowrap",
                }}
              >
                <img src={langicone} alt="sss" />
              </button>                <i className="bi bi-instagram fs-4"></i>
                <i className="bi bi-facebook fs-4"></i>
                <i className="bi bi-twitter fs-4"></i>
                
              </div>
            </div>

            <div className="col-md-3">
              <h5 className="text-center">وسائل التواصل</h5>
              <ul className="list-unstyled text-center">
                <li>إنستجرام</li>
                <li>فيس بوك</li>
                <li>لينكدإن</li>
              </ul>
             
            </div>

            <div className="col-md-3">
              <h5 className="text-center">برنامج التعلم</h5>
              <ul className="list-unstyled text-center">
                <li>خريطة الكورسات</li>
                <li>الأسئلة الشائعة</li>
              </ul>
            </div>

            <div className="col-md-3">
              <h5 className="text-center">كيفية الوصول</h5>
              <ul className="list-unstyled text-center">
                <li>الوصول للكورسات</li>
                <li>تواصل معنا</li>
                <li>تسجيل الدخول</li>
              </ul>
            </div>
          </div>
     
          <div className="col-md-6 text-md-end" style={{ marginLeft: "55%" }}>
  <div className="text-dark p-3 rounded d-flex justify-content-between" style={{ backgroundColor: "#FCD980", width:"55%" }}>
    <div>
      <strong>البريد:</strong>
      <br />
      contact@website.com
    </div>
    <div>
      <strong>رقم الهاتف:</strong>
      <br />
      +6288 999 222 333
    </div>
  </div>
</div>

        </div>
      </footer>
      
      <div className="row align-items-center py-3 bg-light text-dark text-center  p-5">
  <div className="col-md-6 d-flex w-100 justify-content-between">
    <p className="m-0 fw-bold">&copy; Copyright NextGen 2024 - 2025</p>
    <div className="d-flex align-items-end gap-4">
      <p className="m-0">المحتويات</p>
      <p className="m-0">الكورسات</p>
      <p className="m-0">الأسئلة الشائعة</p>
      <p className="m-0">الصفحة الرئيسية</p>
    </div>
  </div>
</div>


    </>
  );
};

export default Footer;
