import React, { useState, useEffect } from 'react';
import axios from 'axios';
import Filter from '../Components/Filter';

const News = () => {
    const [news, setNews] = useState([]);
    const [filters, setFilters] = useState({ date: '', popularity: '', category: '' });

    useEffect(() => {
        fetchNews();
    }, [filters]);

    const fetchNews = () => {
        const params = new URLSearchParams(filters).toString();
        axios.get(`https://localhost:7249/News/filter?${params}`)
            .then(response => setNews(response.data))
            .catch(error => console.error('Error fetching news', error));
            console.log
    };

    const handleFilterChange = (newFilters) => {
        setFilters(newFilters);
    };

    return (
        <div className="news-page">
            <Filter onFilterChange={handleFilterChange} />
            <div className="news-list">
                {news.map((article) => (
                    <div key={article.id} className="news-item">
                        <h3>{article.title}</h3>
                        <p>{article.content}</p>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default News;
