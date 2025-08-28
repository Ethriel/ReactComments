import { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';

import axios from 'axios';
import DOMpurify from 'dompurify'
import { Form, message, Button, Upload } from 'antd';
import ReactQuill from 'react-quill-new';
import 'react-quill-new/dist/quill.snow.css';

import { FileUploader } from '../file-uploader/file-uploader';
import { MAIN } from '../../utility/routes/app-paths';
import { getCurrentUser } from '../../utility/handle-current-user/get-current-user'
import { imageResizer } from '../../utility/image-resizer'

const QuillComponent = ({ handleChange, ...props }) => {
    const toolbarOptions = [
        'link', 'code-block', 'italic', 'bold'
    ]
    return <ReactQuill modules={{
        toolbar: toolbarOptions
    }} theme='snow' onChange={handleChange} />;
}

export const AddComment = () => {
    const [commentText, setCommentText] = useState('');
    const [selectedFile, setSelectedFile] = useState(null);
    const [fileSelected, setFileSelected] = useState(false);
    const [fileList, setFileList] = useState([]);
    const navigate = useNavigate();
    const { id } = useParams();

    const addingReply = id ? true : false;

    const [comment, setComment] = useState({
        id: '',
        text: '',
        file: null,
        parentCommentId: '',
        personId: 0
    });

    const beforeUpload = async (file) => {
        if (file.type === "text/plain") {
            const isLt100KB = file.size <= 100 * 1024;
            if (!isLt100KB) {
                message.error(`${file.name} is larger than 100KB`);
                return Upload.LIST_IGNORE;
            }
        }

        if (file.type.startsWith("image/")) {
            try {
                const resizedFile = await imageResizer(file, 320, 240);
                return resizedFile;
            } catch (err) {
                console.error("Resize failed", err);
                return false;
            }
        }

        return true;
    }

    const onFileChange = (info, resetFile) => {
        const file = info.file.originFileObj;
        setSelectedFile(file)
        setFileSelected(true)
        resetFile?.()
        setFileList(info.fileList);
    }

    const normalizeQuillHtml = (html) => {
        if (!html) return "";

        const parser = new DOMParser();
        const doc = parser.parseFromString(html, "text/html");

        // Convert <em> → <i>
        doc.querySelectorAll("em").forEach((em) => {
            const i = doc.createElement("i");
            i.innerHTML = em.innerHTML;
            em.replaceWith(i);
        });

        // Convert Quill code blocks → <code>
        doc.querySelectorAll("div.ql-code-block").forEach((div) => {
            const code = doc.createElement("code");
            code.innerHTML = div.innerHTML;
            div.replaceWith(code);
        });

        // Remove outer ql-code-block-container but keep children (including <br>)
        doc.querySelectorAll("div.ql-code-block-container").forEach((container) => {
            while (container.firstChild) {
                container.parentNode.insertBefore(container.firstChild, container);
            }
            container.remove();
        });

        return doc.body.innerHTML;
    }

    const handleTextChange = (content, delta, source, editor) => {
        const html = editor.getHTML();
        const normalized = normalizeQuillHtml(html);
        const cleanText = DOMpurify.sanitize(normalized);
        setCommentText(cleanText);
        setComment({
            ...comment,
            text: cleanText
        })
    }
    const onFinish = (values) => {
        const formData = new FormData();
        const data = {
            ...comment,
            personId: getCurrentUser().id,
            parentCommentId: id ? id : ''
        }

        formData.append('id', data.id);
        formData.append('text', commentText);
        if (selectedFile !== null && selectedFile !== undefined)
            formData.append('file', selectedFile, selectedFile.name);
        formData.append('parentCommentId', data.parentCommentId);
        formData.append('personId', data.personId);

        const endpoint = addingReply ? '/api/comment/add-reply' : '/api/comment/add';

        axios.post(endpoint, formData, {
            headers: {
                "Content-Type": "multipart/form-data"
            }
        })
            .then(function (response) {
                navigate(MAIN);
            })
            .catch(function (error) {
                console.error('ERROR', error);
            });
    }

    return (
        <Form
            onFinish={onFinish}
        >
            <Form.Item
                label="Text"
                name="text">
                {/* <Input.TextArea /> */}
                <QuillComponent handleChange={handleTextChange} />
            </Form.Item>
            <Form.Item>
                <FileUploader
                    beforeUpload={beforeUpload}
                    fileSelected={fileSelected}
                    onFileChange={onFileChange}
                    fileList={fileList}
                    multiple={false}
                    accept={['.txt', '.jpg', '.png']}
                />
            </Form.Item>
            <Form.Item style={{ marginBottom: "0px" }}>
                <Button block type="primary" htmlType="submit" style={{ maxWidth: '200px' }}>
                    Submit
                </Button>
            </Form.Item>
        </Form>
    )
}