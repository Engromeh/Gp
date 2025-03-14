import { Link, Links } from "react-router-dom";
import logonav from "../../../../assets/navbarlogo.svg";
import langicone from "../../../../assets/langua icon.svg"
import homeIcon from "../../../../assets/homeicone.svg"
import contentIcon from "../../../../assets/contenyicon.svg"
import infoIcon from "../../../../assets/info icone.svg"
import searchbar from "../../../../assets/searchicon.svg"

const Navbar = () => {
  
  return (
    <nav
      className="navbar navbar-expand-lg navbar-light "
      style={{
    backgroundColor: "#102D4C",
    width: "100%",
    position: "fixed", 
    top: "0", 
    left: "0", 
    zIndex: "1000", 
  }}    >
      <div className="container-fluid d-flex align-items-center justify-content-between">
        <img src={logonav} className="navbar-brand me-2" alt="logo" />

        <button
          className="navbar-toggler ms-1 p-1"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarSupportedContent"
          aria-controls="navbarSupportedContent"
          aria-expanded="false"
          aria-label="Toggle navigation"
          style={{ width: "auto", padding: "5px 8px" }}
        >
          <span className="navbar-toggler-icon"></span>
        </button>

        <div
          className="collapse navbar-collapse justify-content-center w-100"
          id="navbarSupportedContent"
        >
        <form class="d-flex position-relative" style={{width: "50%"}}>
  <input class=" bg-transparent pe-5" 
         type="search" 
         placeholder="البحث" 
         aria-label="Search"
         style={{
      width: "100%",
      borderRadius: "20px",
      direction: "rtl",
      border: "1px solid #ccc",
      height:"40px"
    }}
/>
  
  <img src={searchbar} alt="بحث" 
       class="position-absolute top-50 translate-middle-y me-3" 
       style={{
      width: "20px",
      height: "20px",
      right: "93%",
      position: "absolute"
    }} />
</form>

      <ul className="navbar-nav text-center me-auto mb-2 mb-lg-0 gap-2 p-0 d-flex align-items-center">
            
            <li className="nav-item">
              <Link className="nav-link d-flex align-items-center gap-1"
                style={{ fontSize: "18px", fontWeight: "bold", color: "#D9D9D9" }}
              >
                <img src={homeIcon} alt="الرئيسية" style={{ height: "20px" }} />
                الرئيسية
              </Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link d-flex align-items-center gap-1"
                style={{ fontSize: "18px", fontWeight: "bold", color:"#D9D9D9" }}
              >
                <img src={contentIcon} alt="الرئيسية" style={{ height: "20px" }} />
                المحتويات              </Link>
            </li>
             <li className="nav-item">
              <Link className="nav-link d-flex align-items-center gap-1"
                style={{ fontSize: "18px", fontWeight: "bold" , color:"#D9D9D9" }}
              >
                <img src={infoIcon} alt="الرئيسية" style={{ height: "20px" }} />
                معلومات عنا              </Link>
            </li>
            
          </ul>

          <div className="d-flex align-items-center ms-2">
            
           

            <form className="button-nav d-flex gap-2">
              <Link
                type="submit"
                to={"/login"}
                className="button-nav btn w-100 w-lg-auto"
                style={{
                  color: "#FFFFFF",
                  border: "2px #D9D9D9 solid",
                 
                  padding: "5px 12px",
                  fontSize: "15px",
                  whiteSpace: "nowrap",
                }}
              >
                تسجيل دخول
              </Link>
              <Link
                type="submit"
                to={"/register"}
                className="button-nav btn w-100 w-lg-auto"
                style={{
                  color: "#FFFFFF",
                  border: "2px #D9D9D9 solid",
                 
                  padding: "5px 12px",
                  fontSize: "15px",
                  whiteSpace: "nowrap",
                }}
              >
                تسجيل جديد
              </Link>
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
              </button>
            </form>
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
