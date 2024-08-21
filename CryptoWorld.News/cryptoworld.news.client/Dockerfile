FROM node:22-alpine

WORKDIR /app

COPY cryptoworld.news.client/package.json ./

RUN npm install 

COPY cryptoworld.news.client/ ./

EXPOSE 5173

CMD ["npm", "start"]