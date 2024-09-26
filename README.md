# EarlyVisionAI

## Table of Contents
- [About](#about)
- [Problem Statement](#problem-statement)
- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
- [Privacy and Security](#privacy-and-security)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## About

EarlyVisionAI is an innovative web application designed to assist in the early detection of skin cancer, with a particular focus on African American and other dark-skinned individuals. By leveraging advanced image processing and machine learning techniques, our application aims to improve the accuracy and timeliness of skin cancer diagnoses in populations where traditional methods often fall short.

## Problem Statement

Skin cancer detection in African Americans and individuals with darker skin tones presents unique challenges. According to [Medical News Today](https://www.medicalnewstoday.com/articles/skin-cancer-black-skin), several factors contribute to this issue:

1. Skin cancer lesions on darker skin are often darker in color, making them harder to distinguish from normal skin variations.
2. Many dermatologists lack experience in diagnosing skin cancer on darker skin tones.
3. There's a common misconception that individuals with darker skin are not susceptible to skin cancer.

These factors often lead to delayed diagnoses, resulting in more advanced stages of cancer at the time of detection. Consequently, African Americans are more likely to die from skin cancer compared to other demographics.

EarlyVisionAI addresses these challenges by providing a tool that can analyze images of skin lesions, potentially identifying concerning patterns early on, regardless of skin tone.

## Features

- User-friendly interface for uploading skin images
- Advanced image processing algorithms optimized for various skin tones
- Real-time analysis and immediate results
- Educational resources on skin cancer prevention and detection
- Strict privacy measures ensuring no user data is stored

## Getting Started

### Prerequisites

- Docker
- Docker Compose

### Installation

1. Clone the repository:
   ```
   git clone https://github.com/EarlyVisionAI/EarlyVisionAI.git
   ```

2. Navigate to the project directory:
   ```
   cd EarlyVisionAI
   ```

3. Create a `.env` file in the root directory and add the required environment variables:
   ```
   cp .env.example .env
   ```
   Edit the `.env` file and fill in the actual values for each variable.

4. Build and run the Docker containers:
   ```
   docker-compose up --build
   ```

The application should now be running and accessible at `http://localhost:5080` and `https://localhost:5081`.

### Environment Variables

The following environment variables are required to run the application:

- `Cloudinary__ApiKey`: Your Cloudinary API key
- `Cloudinary__ApiSecret`: Your Cloudinary API secret
- `Cloudinary__Url`: Your Cloudinary URL
- `Gemini__ApiKey`: Your Gemini API key

Make sure to set these variables in your `.env` file before running the application.

## Usage

1. Open your web browser and navigate to the application URL.
2. Upload an image of the skin area you want to analyze.
3. Wait for the analysis to complete.
4. Review the results and any recommendations provided.
5. Consult with a healthcare professional for any concerning findings.

## Privacy and Security

EarlyVisionAI takes user privacy very seriously. We do not store any user data or uploaded images. All analyses are performed in real-time, and data is immediately discarded after the results are provided. For more information, please refer to our [Privacy Policy](src/EarlyVisionAI/Pages/Privacy.cshtml).

## Contact

For any questions or concerns, please open an issue on this repository or contact us at [eziamaknv@gmail.com].