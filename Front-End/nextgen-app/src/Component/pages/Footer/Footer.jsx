import logo from "../../../assets/logofooter.svg"
import twater from "../../../assets/twiter.svg"
import insta from "../../../assets/insta.svg"
import fasebook from "../../../assets/facebook.svg"
const Footer = () => {
  return (
    <>
      <footer
        className="text-white py-5 px-5 w-100 text-center text-md-start d-none d-md-block"
        style={{ backgroundColor: "#102D4C", padding: "20px" ,    width: "100%",
    position: "fixed", 
    bottom: "0", 
    left: "0" }}
      >
        <div className="container-fluid">
          <div className="row text-center text-md-start">
            <div className="col-lg-4 col-md-6 col-sm-12">
              
              
              <div
                className="d-flex "
                style={{ gap: "10%", marginTop: "130px" }}
              >
                <img src={insta} alt="Instagram" width="24" height="24" />
                <img src={fasebook} alt="Facebook" width="24" height="24" />
                <img src={twater} alt="Twitter" width="24" height="24" />
              </div>
            </div>
            <br />
            <div className="col-lg-2 col-md-6 col-sm-12 mt-4 mt-md-0">
              <p
                style={{
                  fontFamily: "Poppins",
                  fontSize: "20px",
                  color: "#D9D9D9",
                }}
              >
وسائل التواصل              </p>
              <ul
                className="list-unstyled "
                style={{
                  fontFamily: "Poppins",
                  fontSize: "16px",
                  color: "#D9D9D9",
                }}
              >
                <li className="liinfooter">الانستجرام</li>

                <li className="liinfooter">فيس بوك</li>

                <li className="liinfooter">لينكدان</li>

              </ul>
            </div>
            <br />
            <div className="col-lg-2 col-md-6 col-sm-12 mt-4 mt-lg-0">
              <p
                style={{
                  fontFamily: "Poppins",
                  fontSize: "20px",
                  color: "#D9D9D9",
                }}
              >
برنامج التعلم              </p>
              <ul
                className="list-unstyled "
                style={{
                  fontFamily: "Poppins",
                  fontSize: "16px",
                  color: "#D9D9D9",
                }}
              >
                <li className="liinfooter">خريطة كورسات</li>

                <li className="liinfooter">الاسئلة الشائعة</li>

             
              </ul>
            </div>
            <br />
            <div className="col-lg-2 col-md-6 col-sm-12 mt-4 mt-lg-0">
              <p
                style={{
                  fontFamily: "Poppins",
                  fontSize: "20px",
                  color: "#D9D9D9",
                }}
              >
كيفية الوصول              </p>

              <ul
                className="list-unstyled "
                style={{
                  fontFamily: "Poppins",
                  fontSize: "16px",
                  color: "#D9D9D9",
                }}
              >
                <li className="liinfooter">الوصول للكورسات</li>

                <li className="liinfooter">تواصل معنا</li>

                <li className="liinfooter">تسجيل الدخول</li>
              
              </ul>
            </div>

            <div className="col-lg-2 col-md-6 col-sm-12 mt-4 mt-lg-0">
            <div className="d-flex align-items-center justify-content-center justify-content-md-start mb-3">
                <img
                  src={logo}
                  alt="ALFARES Logo"
                  className="me-2"
                style={{width: '100%', height:"100%"}}
                />
                
              </div>
              
            </div>
          </div>
          
        </div>
 
      </footer>
     
    </>
  );
};

export default Footer;
