import React from "react";
import "bootstrap-icons/font/bootstrap-icons.min.css";
import logo from "../../../assets/navlogo.svg";

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
            </div>

            <div className="col-md-3">
              <h5 className="text-center">وسائل التواصل</h5>
              <ul className="list-unstyled text-center">
                <li>إنستجرام</li>
                <li>فيس بوك</li>
                <li>لينكدإن</li>
              </ul>
              <div className="d-flex justify-content-center gap-2">
                <i className="bi bi-instagram fs-4"></i>
                <i className="bi bi-facebook fs-4"></i>
                <i className="bi bi-twitter fs-4"></i>
              </div>
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
          <div className="col-md-6 text-md-end" style={{ marginLeft: "40%" }}>
            <div
              className=" text-dark p-3 rounded"
              style={{ backgroundColor: "#FCD980" }}
            >
              <strong>البريد:</strong> contact@website.com
              <br />
              <strong>رقم الهاتف:</strong> +6288 999 222 333
            </div>
          </div>
        </div>
      </footer>
      <div className="row align-items-center py-3 bg-light text-dark text-center">
        <div className=" d-flex justify-content-between col-md-6">
          <p>&copy; Copyright NextGen 2024 - 2025</p>
          <div className=" d-flex align-items-end gap-4  ">
            <p>المحتويات</p>
            <p>الكورسات</p>
            <p>الاسئلة الشائعة</p>
            <p>الصفحة الرئيسية</p>
          </div>
        </div>
      </div>
    </>
  );
};

export default Footer;
