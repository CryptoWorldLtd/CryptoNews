import React, { useEffect, useState, } from 'react';
import axios from 'axios';
import 'react-slick';

const CardSlider = () => {
const url = 'https://localhost:7249/HomeNews/home';
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [current, setCurrent] = useState(0);

    
    useEffect(() => {
                axios.get(url)
                .then(response => {
                    const allData = response.data;
                    const filteredData = filterDataFromLast24Hours(allData);
                    setData(filteredData);
                    setLoading(false);
                })
            .catch (error => {
                console.error('Error fetching data:', error);
                setLoading(false);
            }); 
    }, []);
             const  filterDataFromLast24Hours = (allData) => {
             const currentTime = new Date().getTime();
             const twentyFourHoursAgo = currentTime - (24 * 60 * 60 * 1000);
    
             return allData.filter(data => {
             const itemTime = new Date(data.datePublished).getTime();
             return itemTime <= twentyFourHoursAgo;
             });
};
  return (
    <div className='cards'>
     <h1>Latest Bitcoin & Cryptocurrency News</h1>
    {data.map((item, index) => {
        return (
            <div className='card'>
                <img className='card-img' src={item.imageUrl} key={index}></img>
                <h3 className='card-title' key={index}>{(item.title)}</h3>
                <div className='card-bottom'>
                    <h6 className='card-content' key={index}>{(item.content).substr(0,155) + '....'}</h6>
                </div>
                <button type='submit' className='card-button'>Read More</button>
            </div>
        )
    })}
   </div>
  )
};

export default CardSlider;