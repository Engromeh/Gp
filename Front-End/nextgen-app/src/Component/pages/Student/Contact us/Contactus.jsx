import React from "react";
import loginbg from "../../../../assets/loginbg.svg";

const Contactus = () => {
  return (
    <>
      <div
        className="container-fluid d-flex justify-content-center align-items-center min-vh-100 mt-6"
        style={{ marginTop: "5%" }}
      >
        <div
          className="row  shadow-lg overflow-hidden "
          style={{ width: "65%", backgroundColor: "#102D4D" }}
        >
          <div className="col-md-6 d-flex flex-column justify-content-center align-items-center text-white position-relative">
            <img
              src={loginbg}
              alt="Background Image"
              className="position-absolute top-0 start-0   object-cover"
              style={{ zIndex: 1, height: "85vh", width: "68vh" }}
            />

            <div
              className="position-absolute top-0 start-0 "
              style={{
                backgroundColor: "#212D5D",
                opacity: 0.5,
                zIndex: 2,
                height: "85vh",
                width: "68vh",
              }}
            ></div>

            <div
              className="position-relative z-3"
              style={{ direction: "rtl", bottom: "33%", left: "15%" }}
            >
              <h2>"تواصل الي ما تريده"</h2>
              <p>
                "خدمة تعليم إلكتروني مجانية جاهزة
                <br />
                لمساعدتك على أن تصبح خبيرًا."
              </p>
            </div>
          </div>

          <div className="col-md-6 p-5  text-white">
            <h4 className="fw-bold text-end">تواصل معنا </h4>
            <p className="text-end">
              "تواصل معنا اذا كان لديك استفسار او تريد شيئا ما !"{" "}
            </p>
            <form className="mt-4">
              <div className="mb-3">
                <input
                  type="email"
                  className="form-control  bg-transparent text-white border-secondary"
                  placeholder="الاسم"
                  style={{ direction: "rtl" }}
                />
              </div>
              <div className="mb-3">
                <input
                  type="text"
                  className="form-control  bg-transparent text-white border-secondary"
                  placeholder="رقم الهاتف"
                  style={{ direction: "rtl" }}
                />
              </div>
              <div className="mb-3">
                <input
                  type="email"
                  className="form-control  bg-transparent text-white border-secondary"
                  placeholder="العنوان"
                  style={{ direction: "rtl" }}
                />
              </div>
              <div className="mb-3">
                <input
                  type="email"
                  className="form-control  bg-transparent text-white border-secondary"
                  placeholder="الرسالة"
                  style={{ direction: "rtl", height: "100PX" }}
                />
              </div>
              <button
                type="submit"
                className="btn w-100 text-white fw-bold"
                style={{ backgroundColor: "#FCD980" }}
              >
                تواصل{" "}
              </button>
            </form>
          </div>
        </div>
      </div>
    </>
  );
};

export default Contactus;
