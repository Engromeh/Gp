import React from "react";
import share from "../../../assets/sharenlogin.svg";
import loginbg from "../../../assets/loginbg.svg";

import { Link } from "react-router-dom";
const Login = () => {
  return (
    <div
      className="container-fluid d-flex justify-content-center align-items-center min-vh-100"
      style={{ backgroundColor: "#102D4D" }}
    >
      <Link
        to={"/"}
        className=" d-flex align-items-center gap-1"
        style={{
          fontSize: "20px",
          fontWeight: "bold",
          color: "#D9D9D9",
          position: "absolute",
          top: "19%",
          left: "19%",
        }}
      >
        <img src={share} alt="المحتويات" style={{ height: "10px" }} />
        الصفحة الرئيسية{" "}
      </Link>
      <div
        className="row  shadow-lg  overflow-hidden "
        style={{ width: "65%" , backgroundColor: "#102D4D" , height:"30%"  }}
        >
        <div className="col-md-6 d-flex flex-column justify-content-center align-items-center text-white position-relative">
  <img
    src={loginbg}
    alt="Background Image"
    className="position-absolute top-0 start-0   object-cover"
    style={{ zIndex: 1 ,  height:"53vh" , width:"56vh"}}
  />

  <div
    className="position-absolute top-0 start-0  "
    style={{ backgroundColor: "#212D5D", opacity: 0.5, zIndex: 2, height:"53vh" , width:"56vh"}}
  ></div>

  <div className="position-relative z-3" style={{direction:"rtl" , bottom:"33%", left:"1%"}}>
    <h2>"خطوة أقرب إلى حلمك"
    
    </h2>
    <p>"خدمة تعليم إلكتروني مجانية جاهزة
    <br/>
     لمساعدتك على أن تصبح خبيرًا."</p>
  </div>
</div>


        <div className="col-md-6 p-5  text-white">
          <h4 className="fw-bold text-end">تسجيل الدخول</h4>
          <p className="text-end">
            "أنشئ حسابك الآن وانطلق في رحلة التعلم والتطوير!"
          </p>
          <form className="mt-4">
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
            <button
              type="submit"
              className="btn w-100 text-white fw-bold"
              style={{ backgroundColor: "#FCD980" }}
            >
              تسجيل الدخول
            </button>
            <p className="mt-3 text-center text-light">
              لست لديك حساب؟ <a href="#">أنشئ حسابًا</a>
            </p>
          </form>
        </div>
      </div>
    </div>
  );
};

export default Login;
