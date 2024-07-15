import React, { useState, useEffect } from 'react';
import axios from 'axios';
import Filter from '../Components/Filter';

const News = () => {
    const [news, setNews] = useState([]);
    const [filters, setFilters] = useState({ date: '', popularity: '', category: '' });
    const [page, setPage] = useState(1);
    const [pageSize] = useState(5);
    const [totalItems, setTotalItems] = useState(0);


    useEffect(() => {
        fetchNews();
    }, [filters, page]);

    const fetchNews = (params) => {
        //const params = new URLSearchParams(filters).toString();
            axios.get(`https://localhost:7249/News/filter?CurrentPage=${params}`)
            .then(response => setNews(response.data))
            .catch(error => console.error('Error fetching news', error));      
    };

    const handleFilterChange = (newFilters) => {
        setFilters(newFilters);
        setPage(1);
    };

    const handlePageChange = (newPage) => {
        fetchNews(newPage);
		setPage(newPage);
    };

    const totalPages =10;// Math.ceil(totalItems / pageSize);
	
    return (
        <div className="news-page">
            <Filter onFilterChange={handleFilterChange} />
            <div className="pagination">
                <button onClick={() => handlePageChange(page - 1)} disabled={page === 1}>Previous</button>
                <span>Page {page} of {totalPages}</span>
                <button onClick={() => handlePageChange(page + 1)} disabled={page === totalPages}>Next</button>
            </div>
            <div className="news-list">
                {news.map((article) => (
                    <div key={article.id} className="news-item">
                        <img src={article.imageUrl}  height="500"></img>
                        <h3>{article.title}</h3>
                        <p>{article.content}</p>
                        <p>Date of publication: {article.datePublished}</p>
                        <p>Rating popularity: {article.rating}</p>
                     </div>
                ))}
            </div>
        </div>
    );
};

export default News;
