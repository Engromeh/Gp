import React from 'react'
import profile from "../../../assets/profile mycourses.svg";
import Plus from "../../../assets/plus.svg";
import couresimg from "../../../assets/courseimage.svg";
import hour from "../../../assets/hour.svg";
import video from "../../../assets/videoicon.svg";
import visit from "../../../assets/visit.svg";
import { Link } from 'react-router-dom';
function InsContent() {
    const courses = [
        {
            id: 1,
            category: "تنمية الذات وريادة الأعمال",
            title: "Dasar Pemrograman WEB",
            instructor: "By Hedra Naguib",
            description: "Materi pembelajarn mengenai pembuatan website tingkat pemula",
            hours: "4.5",
            lectures: 20,
            visits: "1,900",
        },
        {
            id: 2,
            category: "تنمية الذات وريادة الأعمال",
            title: "Dasar Pemrograman WEB",
            instructor: "By Hedra Naguib",
            description: "Materi pembelajarn mengenai pembuatan website tingkat pemula",
            hours: "4.5",
            lectures: 20,
            visits: "1,900",
        },
        {
            id: 3,
            category: "تنمية الذات وريادة الأعمال",
            title: "Dasar Pemrograman WEB",
            instructor: "By Hedra Naguib",
            description: "Materi pembelajarn mengenai pembuatan website tingkat pemula",
            hours: "4.5",
            lectures: 20,
            visits: "1,900",
        },
        {
            id: 4,
            category: "تنمية الذات وريادة الأعمال",
            title: "Dasar Pemrograman WEB",
            instructor: "By Hedra Naguib",
            description: "Materi pembelajarn mengenai pembuatan website tingkat pemula",
            hours: "4.5",
            lectures: 20,
            visits: "1,900",
        },
        {
            id: 5,
            category: "تنمية الذات وريادة الأعمال",
            title: "Dasar Pemrograman WEB",
            instructor: "By Hedra Naguib",
            description: "Materi pembelajarn mengenai pembuatan website tingkat pemula",
            hours: "4.5",
            lectures: 20,
            visits: "1,900",
        },
    ];
    return (

        <>
            <div
                className="d-flex flex-row-reverse align-items-center justify-content-between w-100 px-5"
                style={{ marginTop: "9%" }}
            >
                <div className="d-flex align-items-center gap-2">
                    <p className=" text-white  align-items-center mt-3 fw-bold">
                        مرحبا استاذ هدرا , نحن سعداء بعودتك
                    </p>
                    <img src={profile} alt="profile" />
                </div>

                <Link to="/instructor/addCourse" style={{ textDecoration: "none", color: "inherit" }}>
                    <button
                        className="btn d-flex align-items-center"
                        style={{backgroundColor: "#FCD980", gap: "5px", fontWeight: "bold" }}
                    >
                        اضافة كورس
                        <img src={Plus} alt="addCourse" style={{ width: "15px" }} />
                    </button>
                </Link>
                

            </div>


            < div className="container " style={{ marginTop: "4%" }}>
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
                            <Link to="/instructor/editCourse"><button className="addcourse-btn">تعديل الكورس </button></Link>


                        </div>
                    ))}
                </div>
            </div >

        </>
    )
}

export default InsContent

