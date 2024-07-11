import React, { useEffect, useState, } from 'react';
import axios from 'axios';

const CardSlider = () => {
const url = 'https://localhost:7249/News/pastNews';
    const [data, setData] = useState([]);

    useEffect(() => {
        const FetchData = async () => {
            try{
                const response = await axios.get(url)
                setData(response.data)
            }
            catch{
            console.error('Error fetching data:', error)
           };
        };
        FetchData();
            
    }, []);
 
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
                <button type='submit' className='card-button'>Read More</button>
                </div>
            </div>
        )
    })}
   </div>
  )
};

export default CardSlider;