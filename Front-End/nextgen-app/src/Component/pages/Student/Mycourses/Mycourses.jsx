import React from "react";
import couresimg from "../../../../assets/courseimage.svg";
import hour from "../../../../assets/hour.svg";
import video from "../../../../assets/videoicon.svg";
import visit from "../../../../assets/visit.svg";
import profile from "../../../../assets/profile mycourses.svg";

const Mycourses = () => {
  const courses = [
    {
      id: 1,
      category: "تنمية الذات وريادة الأعمال",
      title: "Dasar Pemrograman WEB",
      instructor: "By Hedra Naguib",
      description:
        "Materi pembelajarn mengenai pembuatan website tingkat pemula",
      hours: "4.5",
      lectures: 20,
      visits: "1,900",
    },
    {
      id: 2,
      category: "تنمية الذات وريادة الأعمال",
      title: "Dasar Pemrograman WEB",
      instructor: "By Hedra Naguib",
      description:
        "Materi pembelajarn mengenai pembuatan website tingkat pemula",
      hours: "4.5",
      lectures: 20,
      visits: "1,900",
    },
    {
      id: 3,
      category: "تنمية الذات وريادة الأعمال",
      title: "Dasar Pemrograman WEB",
      instructor: "By Hedra Naguib",
      description:
        "Materi pembelajarn mengenai pembuatan website tingkat pemula",
      hours: "4.5",
      lectures: 20,
      visits: "1,900",
    },
    {
      id: 4,
      category: "تنمية الذات وريادة الأعمال",
      title: "Dasar Pemrograman WEB",
      instructor: "By Hedra Naguib",
      description:
        "Materi pembelajarn mengenai pembuatan website tingkat pemula",
      hours: "4.5",
      lectures: 20,
      visits: "1,900",
    },
    {
      id: 4,
      category: "تنمية الذات وريادة الأعمال",
      title: "Dasar Pemrograman WEB",
      instructor: "By Hedra Naguib",
      description:
        "Materi pembelajarn mengenai pembuatan website tingkat pemula",
      hours: "4.5",
      lectures: 20,
      visits: "1,900",
    },
    {
      id: 4,
      category: "تنمية الذات وريادة الأعمال",
      title: "Dasar Pemrograman WEB",
      instructor: "By Hedra Naguib",
      description:
        "Materi pembelajarn mengenai pembuatan website tingkat pemula",
      hours: "4.5",
      lectures: 20,
      visits: "1,900",
    },
    {
      id: 4,
      category: "تنمية الذات وريادة الأعمال",
      title: "Dasar Pemrograman WEB",
      instructor: "By Hedra Naguib",
      description:
        "Materi pembelajarn mengenai pembuatan website tingkat pemula",
      hours: "4.5",
      lectures: 20,
      visits: "1,900",
    },
    {
      id: 4,
      category: "تنمية الذات وريادة الأعمال",
      title: "Dasar Pemrograman WEB",
      instructor: "By Hedra Naguib",
      description:
        "Materi pembelajarn mengenai pembuatan website tingkat pemula",
      hours: "4.5",
      lectures: 20,
      visits: "1,900",
    },
  ];

  return (
    <>
      <div
        className=" d-flex  align-items-center justify-content-end gap-2"
        style={{ marginTop: "9%", marginRight: "8%" }}
      >
        <p className=" text-white  align-items-center mt-3 fw-bold">
          مرحبا هدرا , نحن سعداء بعودتك
        </p>
        <img src={profile} alt="profile" />
      </div>
      <div className="container " style={{ marginTop: "4%" }}>
        <div className="row">
          {courses.map((course) => (
            <div key={course.id} className="col-md-3 mb-4">
              <div className="card">
                <div
                  className="position-relative"
                  style={{ borderRadius: "12px", overflow: "hidden" }}
                >
                  <img
                    src={couresimg}
                    alt="course"
                    className="w-100"
                    style={{ borderRadius: "12px" }}
                  />
                  <div
                    className="position-absolute d-flex align-items-center px-3 py-1"
                    style={{
                      bottom: "2px",
                      right: "0px",
                      backgroundColor: "#252641",
                      color: "#fff",
                      borderRadius: "20px",
                      fontWeight: "bold",
                    }}
                  >
                    ⭐ 4.9
                  </div>
                </div>

                <div className="card-body">
                  <p style={{ fontSize: "20px", fontWeight: "bold" }}>
                    {course.title}
                    <span style={{ color: "#2405F2" }}>
                      {" "}
                      {course.instructor}
                    </span>
                  </p>
                  <p className="card-text">{course.description}</p>
                  <div className="d-flex justify-content-between">
                    <p
                      className="d-flex align-items-center gap-1"
                      style={{ fontWeight: "bold", color: "#282938" }}
                    >
                      <img src={hour} alt="ساعات" style={{ height: "20px" }} />{" "}
                      {course.hours} ساعات
                    </p>
                    <p
                      className="d-flex align-items-center gap-1"
                      style={{ fontWeight: "bold", color: "#282938" }}
                    >
                      <img
                        src={video}
                        alt="محاضرات"
                        style={{ height: "20px" }}
                      />{" "}
                      {course.lectures} محاضرة
                    </p>
                    <p
                      className="d-flex align-items-center gap-1"
                      style={{ fontWeight: "bold", color: "#282938" }}
                    >
                      <img src={visit} alt="زيارات" /> {course.visits} زيارة
                    </p>
                  </div>
                </div>
              </div>
              <button className="addcourse-btn">اضافة الكورس </button>
            </div>
          ))}
        </div>
      </div>
    </>
  );
};

export default Mycourses;
