import { useRef, useState } from 'react'



import { Button, Upload } from 'antd'
import { LoadingOutlined, PlusOutlined, UploadOutlined } from '@ant-design/icons';
import './styles.css'

export const FileUploader = ({
    onFileChange,
    beforeUpload,
    fileList,
    fileSelected = false,
    multiple = false,
    accept = '.txt',
    ...props
}) => {
    const [loading, setLoading] = useState(false);
    
    const inputRef = useRef()
    const resetFile = () => {
        inputRef.current.value = null
    }

    const uploadButton = (
        <button style={{ border: 0, background: 'none' }} type="button">
            {loading ? <LoadingOutlined /> : <PlusOutlined />}
            <div style={{ marginTop: 8, color: 'black' }}>Upload file</div>
        </button>
    );

    return (
        <>
            <Upload
                beforeUpload={beforeUpload}
                id="inputFile"
                onChange={(info) => onFileChange?.(info, resetFile)}
                listType="picture-card"
                // type='file'
                multiple={multiple}
                customRequest={({ onSuccess }) => {
                    setTimeout(() => {
                        onSuccess('ok');
                    }, 0);
                }}
                ref={inputRef}
                fileList={fileList}
                accept={accept}
                className="avatar-uploader"
            >
                Upload file
            </Upload>
        </>
    )
}
