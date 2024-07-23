import React from "react";
import './Home.css'
import NewsSlider from "../Components/NewsSlider";
import CardSlider from '../Components/CardSlider';

const Home = () => {
    return (
        <div>
            <NewsSlider/>
            <div>
            <CardSlider/>
            </div>
        </div>
    )
};
    
export default Home;
