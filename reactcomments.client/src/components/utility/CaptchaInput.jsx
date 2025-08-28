import { useRef, useImperativeHandle } from 'react';

import ReCAPTCHA from 'react-google-recaptcha';

import './styles.css';

export const CaptchaInput = ({ value, onChange }, ref) => {
    const captchaRef = useRef(null);

    const handleCaptchaChange = (token) => {
        onChange?.(token);
    };

    useImperativeHandle(ref, () => ({
        reset: () => {
            captchaRef.current?.reset();
            onChange?.(null);
        },
    }));

    return (
        <div className='form-captcha-div'>
            <ReCAPTCHA
                ref={captchaRef}
                sitekey="6LdLDLMrAAAAAO1cC-gYTyBGiNyOj9AgdWmc-sp8"
                onChange={handleCaptchaChange}
            />
        </div>
    );
};