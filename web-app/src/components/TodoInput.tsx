import { Input } from 'antd'
import { useState } from 'react'

export interface SubmitResult {
    isSuccesful: boolean;
    ErrorMessage?: string;
}

export interface TodoInputProps {
    placeHolderText?: string;
    onSubmit (value: string): Promise<SubmitResult>;
}

function TodoInput (props:TodoInputProps)  {

    const [inputValue, setInputValue] = useState('')

    const submit = async () => {
        const result = await props.onSubmit(inputValue);
        if (result.isSuccesful)
            setInputValue('');
    }

    const onKeyEnter = async (key:string) => {
        if (key == "Enter")
            submit();        
    }

    return (
        <>
          <Input 
            value={inputValue} 
            onChange={e => setInputValue(e.currentTarget.value)}
            placeholder={props.placeHolderText} 
            onKeyDown={e=>onKeyEnter(e.key)}
            maxLength={100}
            size="large"  />  
        </>
    )
}

export default TodoInput
