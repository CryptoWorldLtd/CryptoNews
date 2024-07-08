import React, { useEffect, useState, } from 'react';
import {FaArrowAltCircleRight, FaArrowAltCircleLeft} from 'react-icons/fa';
import axios from 'axios';

const NewsSlider = () => {
    const url = 'https://localhost:7249/HomeNews/home';
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [current, setCurrent] = useState(0);

    const nextSlide = () => {
        setCurrent(current === data.length - 1 ? 0 : current + 1)
    }

    const previosSlide = () => {
        setCurrent(current === 0 ? data.length - 1 : current - 1)
    }

    
    useEffect(() => {
        const FetchData = async () => {
            try{
                const response = await axios.get(url)
                setData(response.data);
            }
            catch{
            console.error('Error fetching data:', error);
           }
        };
        FetchData();
    
    }, []);

return (
    <div className='carousel'>
        <FaArrowAltCircleLeft className='arrow left-side' onClick={previosSlide} />
        {data.map((item, index) => {
            return (
                <div className={index === current ? 'slide-active' : 'slide'} 
                key={index}
                >
                   {index === current && (
                       <img src={item.imageUrl} alt='news' className='image'></img> 
                   )}
                              <div className='content'>
                                     <ul>
                                       <li key={index}>
                                        <h1>{item.title}</h1>
                                       </li>
                                       <li key={index}>
                                         <h5>{(item.content).substr(0,330) + '....'}</h5>
                                        <h6 key={index} className='date'>
                                           {item.datePublished}
                                        </h6>
                                       <button type='submit' className='button'>Read More</button>
                                       </li>
                                    </ul>
                               </div>
                   </div>
            )
        })}
        <FaArrowAltCircleRight className='arrow right-side' onClick={nextSlide}/>
        <span className='indicators'>
            {data.map((_, index) => {
                return <button key={index} className={current === index ? 'indicator' : 'indicator indicator-inactive'} onClick={() => setCurrent(index)}></button>
            })}
        </span>
    </div>
    );
};
  
  export default NewsSlider;