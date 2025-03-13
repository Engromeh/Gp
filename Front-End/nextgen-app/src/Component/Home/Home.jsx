import React from 'react'
import Navbar from "../pages/Navbar/Navbar"
import Hero from '../pages/Hero/Hero'
import AboutUs from '../pages/AboutUs/AboutUs'
import Contente from '../pages/Contente/Contente'
import Footer from '../pages/Footer/Footer'
const Home = () => {
  return (
    <>
      <Navbar />
      <Hero />
      <AboutUs />
      <Contente />
      <Footer/>
    </>
  )
}

export default Home
