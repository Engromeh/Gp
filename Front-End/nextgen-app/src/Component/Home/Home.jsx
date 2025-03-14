import React from 'react'
import Navbar from "../pages/HomePage/Navbar/Navbar"
import Hero from '../pages/HomePage/Hero/Hero'
import AboutUs from '../pages/HomePage/AboutUs/AboutUs'
import Contente from '../pages/HomePage/Contente/Contente'
import Footer from '../pages/HomePage/Footer/Footer'
import Hero2 from '../pages/HomePage/Hero2/Hero2'
const Home = () => {
  return (
    <>
      <Navbar />
      <Hero />
      <AboutUs />
      <Contente />
      <Hero2 />
      <Footer/>
    </>
  )
}

export default Home
